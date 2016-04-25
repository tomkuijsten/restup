using Restup.HttpMessage;
using Restup.Webserver.Models.Schemas;
using System;
using System.Linq;

namespace Restup.Webserver.Rest
{
    internal class RestServerRequestFactory
    {
        public RestServerRequest Create(IHttpServerRequest httpRequest)
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

        private MediaType GetContentMediaType(IHttpServerRequest httpRequest)
        {
            var contentMediaType = GetMediaType(httpRequest.ContentType ?? string.Empty); // guard against nulls
            if (contentMediaType == MediaType.Unsupported)
                return Configuration.Default.DefaultContentType;

            return contentMediaType;
        }

        private static MediaType GetMediaType(string contentType)
        {
            if ("application/json".Equals(contentType, StringComparison.OrdinalIgnoreCase) ||
                "text/json".Equals(contentType, StringComparison.OrdinalIgnoreCase))
                return MediaType.JSON;

            if ("application/xml".Equals(contentType, StringComparison.OrdinalIgnoreCase) ||
                "text/xml".Equals(contentType, StringComparison.OrdinalIgnoreCase))
                return MediaType.XML;

            return MediaType.Unsupported;
        }

        private string GetContentCharset(IHttpServerRequest httpRequest, MediaType contentMediaType)
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

        private MediaType GetAcceptMediaType(IHttpServerRequest httpRequest)
        {
            var preferredType = httpRequest.AcceptMediaTypes
                                    .Select(GetMediaType)
                                    .FirstOrDefault(a => a != MediaType.Unsupported);

            if (preferredType == MediaType.Unsupported)
                preferredType = Configuration.Default.DefaultAcceptType;

            return preferredType;
        }

        private string GetAcceptCharset(IHttpServerRequest httpRequest, MediaType acceptMediaType)
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
