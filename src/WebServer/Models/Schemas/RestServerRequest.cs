using Devkoes.HttpMessage;
using Devkoes.HttpMessage.Models.Schemas;
using System;
using System.Linq;
using System.Text;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    internal class RestServerRequest
    {
        internal HttpServerRequest HttpServerRequest { get; }

        public RestServerRequest(HttpServerRequest request)
        {
            HttpServerRequest = request;

            InitializeAcceptMediaType();

            InitializeAcceptCharset();

            InitializeContentMediaType();

            InitializeContentEncoding();
        }

        private void InitializeContentMediaType()
        {
            if (HttpServerRequest.ContentType.GetValueOrDefault() == MediaType.Unsupported)
            {
                ContentMediaType = Configuration.Default.DefaultContentType;
            }
            else
            {
                ContentMediaType = HttpServerRequest.ContentType.Value;
            }
        }

        private void InitializeContentEncoding()
        {
            var requestContentCharset = HttpServerRequest.ContentTypeCharset;
            var encoding = EncodingCache.Default.GetEncoding(requestContentCharset);
            if (encoding == null)
            {
                if (ContentMediaType == MediaType.JSON)
                {
                    requestContentCharset = Configuration.Default.DefaultJSONCharset;
                }
                else if (ContentMediaType == MediaType.XML)
                {
                    requestContentCharset = Configuration.Default.DefaultXMLCharset;
                }
                else
                {
                    throw new NotImplementedException("Content media type is not supported.");
                }
            }

            ContentCharset = requestContentCharset;
            ContentEncoding = EncodingCache.Default.GetEncoding(requestContentCharset);
        }

        private void InitializeAcceptMediaType()
        {
            var preferredType = HttpServerRequest.AcceptMediaTypes.FirstOrDefault(a => a != MediaType.Unsupported);

            if (preferredType == MediaType.Unsupported)
            {
                preferredType = Configuration.Default.DefaultAcceptType;
            }

            AcceptMediaType = preferredType;
        }

        private void InitializeAcceptCharset()
        {
            string firstAvailableEncoding = null;

            foreach (var requestedCharset in HttpServerRequest.AcceptCharsets)
            {
                var encoding = EncodingCache.Default.GetEncoding(requestedCharset);
                firstAvailableEncoding = requestedCharset;

                if (encoding != null)
                    break;
            }

            if (string.IsNullOrEmpty(firstAvailableEncoding))
            {
                if (AcceptMediaType == MediaType.JSON)
                {
                    firstAvailableEncoding = Configuration.Default.DefaultJSONCharset;
                }
                else if (AcceptMediaType == MediaType.XML)
                {
                    firstAvailableEncoding = Configuration.Default.DefaultXMLCharset;
                }
                else
                {
                    throw new NotImplementedException("Accept media type is not supported.");
                }
            }

            AcceptCharset = firstAvailableEncoding;
            AcceptEncoding = EncodingCache.Default.GetEncoding(firstAvailableEncoding);
        }

        internal string AcceptCharset { get; private set; }

        internal MediaType AcceptMediaType { get; private set; }

        internal Encoding AcceptEncoding { get; private set; }

        internal MediaType ContentMediaType { get; private set; }

        internal string ContentCharset { get; private set; }

        internal Encoding ContentEncoding { get; private set; }
    }
}
