using Devkoes.HttpMessage.Headers;
using Devkoes.HttpMessage.Headers.Response;
using Devkoes.HttpMessage.Models.Contracts;
using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.HttpMessage.Plumbing;
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
        private string _content;

        internal IEnumerable<IHttpHeader> Headers => _headers;

        // Header line info
        public Version HttpVersion { get; set; }
        public HttpResponseStatus ResponseStatus { get; set; }

        // Properties for the content
        public int ContentLength => Content?.Length ?? 0;

        internal HttpServerResponse(Version httpVersion, HttpResponseStatus status)
        {
            _headers = new List<IHttpHeader>();

            HttpVersion = httpVersion;
            ResponseStatus = status;
        }

        public static HttpServerResponse Create(HttpResponseStatus status)
        {
            return Create(new Version(1, 1), status);
        }

        public static HttpServerResponse Create(Version httpVersion, HttpResponseStatus status)
        {
            return new HttpServerResponse(httpVersion, status);
        }

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;

                var contentLengthHeader = Headers.OfType<ContentLengthHeader>().SingleOrDefault();
                _headers.Remove(contentLengthHeader);

                if (!string.IsNullOrEmpty(value))
                {
                    _headers.Add(new ContentLengthHeader(value.Length));
                }
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

        internal Encoding ContentTypeEncoding
        {
            get
            {
                return Headers.OfType<ContentTypeHeader>().SingleOrDefault()?.Encoding ?? Constants.DefaultHttpEncoding;
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
                    contentTypeHeader = new ContentTypeHeader(MediaType.Unsupported, value);
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
    }
}
