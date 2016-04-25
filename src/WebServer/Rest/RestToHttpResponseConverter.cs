using Restup.HttpMessage;
using Restup.Webserver.Http;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;

namespace Restup.Webserver.Rest
{
    internal class RestToHttpResponseConverter
    {
        private readonly ContentSerializer _contentSerializer;

        public RestToHttpResponseConverter()
        {
            _contentSerializer = new ContentSerializer();
        }

        internal HttpServerResponse ConvertToHttpResponse(IRestResponse restResponse, RestServerRequest restServerRequest)
        {
            var methodNotAllowedResponse = restResponse as MethodNotAllowedResponse;
            if (methodNotAllowedResponse != null)
                return GetMethodNotAllowedResponse(methodNotAllowedResponse, restServerRequest);

            var postResponse = restResponse as PostResponse;
            if (postResponse != null)
                return GetPostResponse(postResponse, restServerRequest);

            var response = restResponse as IContentRestResponse;
            if (response != null)
                return GetDefaultContentResponse(response, restServerRequest);

            return GetDefaultResponse(restResponse);
        }

        private static HttpServerResponse GetMethodNotAllowedResponse(MethodNotAllowedResponse methodNotAllowedResponse, RestServerRequest restReq)
        {
            var serverResponse = GetDefaultResponse(methodNotAllowedResponse);
            serverResponse.Allow = methodNotAllowedResponse.Allows;

            return serverResponse;
        }

        private HttpServerResponse GetPostResponse(PostResponse response, RestServerRequest restReq)
        {
            var serverResponse = GetDefaultContentResponse(response, restReq);

            if (response.Status == PostResponse.ResponseStatus.Created)
                serverResponse.Location = new Uri(response.LocationRedirect, UriKind.RelativeOrAbsolute);

            return serverResponse;
        }

        private HttpServerResponse GetDefaultContentResponse(IContentRestResponse response, RestServerRequest restReq)
        {
            var defaultResponse = GetDefaultResponse(response);

            if (response.ContentData != null)
            {
                defaultResponse.ContentType = GetMediaTypeAsString(restReq.AcceptMediaType);
                defaultResponse.ContentCharset = restReq.AcceptCharset;
                defaultResponse.Content = _contentSerializer.ToAcceptContent(response.ContentData, restReq);
            }

            return defaultResponse;
        }

        private static HttpServerResponse GetDefaultResponse(IRestResponse response)
        {
            var serverResponse = HttpServerResponse.Create(response.StatusCode);
            serverResponse.Date = DateTime.Now;
            serverResponse.IsConnectionClosed = true;

            return serverResponse;
        }

        private string GetMediaTypeAsString(MediaType acceptMediaType)
        {
            switch (acceptMediaType)
            {
                case MediaType.JSON:
                    return "application/json";
                case MediaType.XML:
                    return "application/xml";
                case MediaType.Unsupported:
                    return "";
            }

            throw new ArgumentException($"Don't know how to convert {nameof(acceptMediaType)}.");
        }
    }
}
