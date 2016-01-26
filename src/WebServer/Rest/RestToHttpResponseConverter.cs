using Devkoes.HttpMessage;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Models.Contracts;
using Devkoes.Restup.WebServer.Models.Schemas;
using System;

namespace Devkoes.Restup.WebServer.Rest
{
    internal class RestToHttpResponseConverter : IRestResponseVisitor<RestServerRequest, HttpServerResponse>
    {
        private ContentSerializer _contentSerializer;

        public RestToHttpResponseConverter()
        {
            _contentSerializer = new ContentSerializer();
        }

        public HttpServerResponse Visit(DeleteResponse response, RestServerRequest restReq)
        {
            return GetDefaultResponse(response);
        }

        public HttpServerResponse Visit(PostResponse response, RestServerRequest restReq)
        {
            var serverResponse = GetDefaultContentResponse(response, restReq);

            if (response.Status == PostResponse.ResponseStatus.Created)
                serverResponse.Location = new Uri(response.LocationRedirect, UriKind.RelativeOrAbsolute);

            return serverResponse;
        }

        public HttpServerResponse Visit(GetResponse response, RestServerRequest restReq)
        {
            return GetDefaultContentResponse(response, restReq);
        }

        public HttpServerResponse Visit(PutResponse response, RestServerRequest restReq)
        {
            return GetDefaultContentResponse(response, restReq);
        }

        public HttpServerResponse Visit(StatusOnlyResponse response, RestServerRequest restReq)
        {
            return GetDefaultResponse(response);
        }

        public HttpServerResponse Visit(MethodNotAllowedResponse methodNotAllowedResponse, RestServerRequest restReq)
        {
            var serverResponse = GetDefaultResponse(methodNotAllowedResponse);
            serverResponse.Allow = methodNotAllowedResponse.Allows;

            return serverResponse;
        }

        private HttpServerResponse GetDefaultContentResponse(IContentRestResponse response, RestServerRequest restReq)
        {
            var defaultResponse = GetDefaultResponse(response);

            if (response.ContentData != null)
            {
                defaultResponse.ContentType = restReq.AcceptMediaType;
                defaultResponse.ContentCharset = restReq.AcceptCharset;
                defaultResponse.Content = _contentSerializer.ToAcceptContent(response.ContentData, restReq);
            }

            return defaultResponse;
        }

        private HttpServerResponse GetDefaultResponse(IRestResponse response)
        {
            var serverResponse = HttpServerResponse.Create(response.StatusCode);
            serverResponse.Date = DateTime.Now;
            serverResponse.IsConnectionClosed = true;

            return serverResponse;
        }
    }
}
