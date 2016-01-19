using Devkoes.HttpMessage.Headers;
using Devkoes.HttpMessage.Headers.Response;
using Devkoes.HttpMessage.Models.Contracts;
using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.HttpMessage.ServerResponseParsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Devkoes.HttpMessage
{
    public class HttpServerResponse
    {
        private List<IHttpHeader> _headers;

        internal HttpServerResponse(Version httpVersion, HttpResponseStatus status)
        {
            _headers = new List<IHttpHeader>();

            HttpVersion = httpVersion;
            ResponseStatus = status;
        }

        public static HttpServerResponse Create(
            Version httpVersion,
            HttpResponseStatus status)
        {
            return new HttpServerResponse(httpVersion, status);
        }

        public void AddHeader(string name, string value)
        {
            var knownHeader = Headers.SingleOrDefault(h => string.Equals(h.Name, name, StringComparison.OrdinalIgnoreCase));
            _headers.Remove(knownHeader);
            _headers.Add(new UntypedResponseHeader(name, value));
        }

        public void AddHeader(IHttpHeader header)
        {
            var knownHeader = Headers.SingleOrDefault(h => string.Equals(h.Name, header.Name, StringComparison.OrdinalIgnoreCase));
            _headers.Remove(knownHeader);
            _headers.Add(header);
        }

        internal IEnumerable<IHttpHeader> Headers => _headers;

        // Header line info
        public Version HttpVersion { get; set; }
        public HttpResponseStatus ResponseStatus { get; set; }

        // Properties for the content
        public int ContentLength => Content?.Length ?? 0;
        public string Content { get; set; }

        // This section contains shortcuts to headers. By setting the property a header is added
        // or removed from the header collection.

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

        public Encoding ContentEncoding
        {
            get
            {
                return Headers.OfType<ContentTypeHeader>().SingleOrDefault()?.Encoding;
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
                    contentTypeHeader = new ContentTypeHeader(MediaType.JSON, value);
                    _headers.Add(contentTypeHeader);
                }
                else
                {
                    contentTypeHeader.Charset = value;
                }
            }
        }

        public MediaType? ContentType
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
                else if (value.HasValue && contentTypeHeader == null)
                {
                    contentTypeHeader = new ContentTypeHeader(value.Value, null);
                    _headers.Add(contentTypeHeader);
                }
                else if (value.HasValue)
                {
                    contentTypeHeader.ContentType = value.Value;
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
            return HttpServerResponseParser.Default.ConvertToString(this);
        }
    }
}
