using Devkoes.HttpMessage;
using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.Restup.WebServer.Models.Schemas;
using System;
using System.Linq;

namespace Devkoes.Restup.WebServer.Rest
{
    internal class RestServerRequestFactory
    {
        public RestServerRequest Create(HttpServerRequest httpRequest)
        {
            var acceptMediaType = GetAcceptMediaType(httpRequest);

            var acceptCharset = GetAcceptCharset(httpRequest, acceptMediaType);

            var contentMediaType = GetContentMediaType(httpRequest);

            var contentCharset = GetContentCharset(httpRequest, contentMediaType);

            return new RestServerRequest(
                httpRequest,
                acceptCharset,
                acceptMediaType,
                EncodingCache.Default.GetEncoding(acceptCharset),
                contentMediaType,
                contentCharset,
                EncodingCache.Default.GetEncoding(contentCharset)
            );
        }

        private MediaType GetContentMediaType(HttpServerRequest httpRequest)
        {
            if (httpRequest.ContentType.GetValueOrDefault() == MediaType.Unsupported)
            {
                return Configuration.Default.DefaultContentType;
            }
            else
            {
                return httpRequest.ContentType.Value;
            }
        }

        private string GetContentCharset(HttpServerRequest httpRequest, MediaType contentMediaType)
        {
            var requestContentCharset = httpRequest.ContentTypeCharset;
            var encoding = EncodingCache.Default.GetEncoding(requestContentCharset);
            if (encoding == null)
            {
                if (contentMediaType == MediaType.JSON)
                {
                    requestContentCharset = Configuration.Default.DefaultJSONCharset;
                }
                else if (contentMediaType == MediaType.XML)
                {
                    requestContentCharset = Configuration.Default.DefaultXMLCharset;
                }
                else
                {
                    throw new NotImplementedException("Content media type is not supported.");
                }
            }

            return requestContentCharset;
        }

        private MediaType GetAcceptMediaType(HttpServerRequest httpRequest)
        {
            var preferredType = httpRequest.AcceptMediaTypes.FirstOrDefault(a => a != MediaType.Unsupported);

            if (preferredType == MediaType.Unsupported)
            {
                preferredType = Configuration.Default.DefaultAcceptType;
            }

            return preferredType;
        }

        private string GetAcceptCharset(HttpServerRequest httpRequest, MediaType acceptMediaType)
        {
            string firstAvailableEncoding = null;

            foreach (var requestedCharset in httpRequest.AcceptCharsets)
            {
                var encoding = EncodingCache.Default.GetEncoding(requestedCharset);
                firstAvailableEncoding = requestedCharset;

                if (encoding != null)
                    break;
            }

            if (string.IsNullOrEmpty(firstAvailableEncoding))
            {
                if (acceptMediaType == MediaType.JSON)
                {
                    firstAvailableEncoding = Configuration.Default.DefaultJSONCharset;
                }
                else if (acceptMediaType == MediaType.XML)
                {
                    firstAvailableEncoding = Configuration.Default.DefaultXMLCharset;
                }
                else
                {
                    throw new NotImplementedException("Accept media type is not supported.");
                }
            }

            return firstAvailableEncoding;
        }
    }
}
