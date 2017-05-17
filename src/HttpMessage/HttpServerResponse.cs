using Restup.HttpMessage.Headers;
using Restup.HttpMessage.Headers.Response;
using Restup.HttpMessage.Models.Contracts;
using Restup.HttpMessage.Models.Schemas;
using Restup.HttpMessage.ServerResponseParsers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restup.HttpMessage
{
    public class HttpServerResponse
    {
        private readonly List<IHttpHeader> _headers;

        internal IEnumerable<IHttpHeader> Headers => _headers;

        // Header line info
        public Version HttpVersion { get; set; }
        public HttpResponseStatus ResponseStatus { get; set; }

        // Content
        public byte[] Content { get; set; }

        private HttpServerResponse(Version httpVersion, HttpResponseStatus status)
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
            // This is just used for debugging purposes and will not be available when running in release mode.
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

        /// <summary>
        /// Will update header if a header with the same name already exists.
        /// </summary>
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
    }
}
