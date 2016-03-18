﻿using Devkoes.HttpMessage.Headers;
using Devkoes.HttpMessage.Headers.Response;
using Devkoes.HttpMessage.Models.Contracts;
using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.HttpMessage.ServerResponseParsers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Devkoes.HttpMessage
{
    public class HttpServerResponse
    {
        private List<IHttpHeader> _headers;
        private byte[] _content;

        internal IEnumerable<IHttpHeader> Headers => _headers;

        // Header line info
        public Version HttpVersion { get; set; }
        public HttpResponseStatus ResponseStatus { get; set; }

        public HttpServerResponse(Version httpVersion, HttpResponseStatus status)
        {
            _headers = new List<IHttpHeader>();

            HttpVersion = httpVersion;
            ResponseStatus = status;
        }

        public static HttpServerResponse Create(int statusCode)
        {
            return Create((HttpResponseStatus)statusCode);
        }

        public static HttpServerResponse Create(HttpResponseStatus status)
        {
            return Create(new Version(1, 1), status);
        }

        public static HttpServerResponse Create(Version httpVersion, HttpResponseStatus status)
        {
            return new HttpServerResponse(httpVersion, status);
        }

        public byte[] Content
        {
            get { return _content; }
            set
            {
                _content = value;

                ResetContentLength();
            }
        }

        public int ContentLength
        {
            get
            {
                if (Content == null)
                    return 0;

                return Content.Length;
            }
        }

        // This section contains shortcuts to headers. By setting the property a header is added,
        // removed or updated from the header collection.

        public DateTime? Date
        {
            get
            {
                return Headers.OfType<DateHeader>().SingleOrDefault()?.Date;
            }
            set
            {
                var dateHeader = Headers.OfType<DateHeader>().SingleOrDefault();
                _headers.Remove(dateHeader);

                if (value.HasValue)
                {
                    _headers.Add(new DateHeader(value.Value));
                }
            }
        }

        public bool IsConnectionClosed
        {
            get
            {
                return Headers.OfType<CloseConnectionHeader>().Any();
            }
            set
            {
                var closeConnHeader = Headers.OfType<CloseConnectionHeader>().SingleOrDefault();
                if (value && closeConnHeader == null)
                {
                    _headers.Add(new CloseConnectionHeader());
                }
                else if (!value && closeConnHeader != null)
                {
                    _headers.Remove(closeConnHeader);
                }
            }
        }

        public string ContentCharset
        {
            get
            {
                return Headers.OfType<ContentTypeHeader>().SingleOrDefault()?.Charset;

            }
            set
            {
                var contentTypeHeader = Headers.OfType<ContentTypeHeader>().SingleOrDefault();
                if (contentTypeHeader == null)
                {
                    contentTypeHeader = new ContentTypeHeader(string.Empty, value);
                    _headers.Add(contentTypeHeader);
                }
                else
                {
                    _headers.Remove(contentTypeHeader);
                    _headers.Add(new ContentTypeHeader(contentTypeHeader.ContentType, value));
                }

                // We should reset the content length, because the charset determines the encoding length
                ResetContentLength();
            }
        }

        public string ContentType
        {
            get
            {
                return Headers.OfType<ContentTypeHeader>().SingleOrDefault()?.ContentType;
            }
            set
            {
                var contentTypeHeader = Headers.OfType<ContentTypeHeader>().SingleOrDefault();
                if (value == null && contentTypeHeader != null)
                {
                    _headers.Remove(contentTypeHeader);
                }
                else if (!string.IsNullOrWhiteSpace(value) && contentTypeHeader == null)
                {
                    contentTypeHeader = new ContentTypeHeader(value, null);
                    _headers.Add(contentTypeHeader);
                }
                else if (!string.IsNullOrWhiteSpace(value))
                {
                    _headers.Remove(contentTypeHeader);
                    _headers.Add(new ContentTypeHeader(value, contentTypeHeader.Charset));
                }

                // We should reset the length, because the default encoder is based on contenttype
                ResetContentLength();
            }
        }

        public Uri Location
        {
            get
            {
                return Headers.OfType<LocationHeader>().SingleOrDefault()?.Location;
            }
            set
            {
                var locationHeader = Headers.OfType<LocationHeader>().SingleOrDefault();
                _headers.Remove(locationHeader);

                if (value != null)
                {
                    _headers.Add(new LocationHeader(value));
                }
            }
        }

        public IEnumerable<HttpMethod> Allow
        {
            get
            {
                var allowHeader = Headers.OfType<AllowHeader>().SingleOrDefault();

                return allowHeader?.Allows ?? Enumerable.Empty<HttpMethod>();
            }
            set
            {
                var allowHeader = Headers.OfType<AllowHeader>().SingleOrDefault();
                _headers.Remove(allowHeader);

                if (value != null)
                {
                    _headers.Add(new AllowHeader(value));
                }
            }
        }

        public byte[] ToBytes()
        {
            return HttpServerResponseParser.Default.ConvertToBytes(this);
        }

        public override string ToString()
        {
#if DEBUG
            // This is just used for debugging purposes and will not be available when running in release mode. Problem with
            // this method is that it uses Encoding to decode the content which is a fairly complicated process. For debugging
            // purposes I'm using UTF-8 which is working most of the time. In real life you want to use the charset provided, or
            // some default encoding as explained in the HTTP specs.
            return HttpServerResponseParser.Default.ConvertToString(this);
#else
            return $"{HttpVersion} {ResponseStatus} including {Headers.Count()} headers.";
#endif
        }

        public IHttpHeader AddHeader(string name, string value)
        {
            var knownHeader = Headers.SingleOrDefault(h => string.Equals(h.Name, name, StringComparison.OrdinalIgnoreCase));
            _headers.Remove(knownHeader);

            var newHeader = new UntypedResponseHeader(name, value);
            _headers.Add(newHeader);

            return newHeader;
        }

        public void AddHeader(IHttpHeader header)
        {
            var knownHeader = Headers.SingleOrDefault(h => string.Equals(h.Name, header.Name, StringComparison.OrdinalIgnoreCase));
            _headers.Remove(knownHeader);
            _headers.Add(header);
        }

        public void RemoveHeader(string name)
        {
            var knownHeader = Headers.SingleOrDefault(h => string.Equals(h.Name, name, StringComparison.OrdinalIgnoreCase));
            _headers.Remove(knownHeader);
        }

        public void RemoveHeader(IHttpHeader header)
        {
            _headers.Remove(header);
        }

        private void ResetContentLength()
        {
            var contentLengthHeader = Headers.OfType<ContentLengthHeader>().SingleOrDefault();
            _headers.Remove(contentLengthHeader);

            if (Content != null && Content.Any())
            {
                _headers.Add(new ContentLengthHeader(ContentLength));
            }
        }
    }
}
