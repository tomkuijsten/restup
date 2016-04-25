using Restup.HttpMessage;
using Restup.Webserver.Models.Contracts;
using System;
using System.Threading.Tasks;

namespace Restup.Webserver.Http
{
    internal class RouteRegistration : IComparable<RouteRegistration>
    {
        private readonly IRouteHandler _routeHandler;
        private readonly string _urlPrefix;

        public RouteRegistration(string urlPrefix, IRouteHandler routeHandler)
        {
            this._urlPrefix = urlPrefix.FormatRelativeUri();
            this._routeHandler = routeHandler;
        }

        public bool Match(IHttpServerRequest request)
        {
            return request.Uri.ToString().StartsWith(_urlPrefix, StringComparison.OrdinalIgnoreCase);
        }

        private static IHttpServerRequest CreateHttpRequestWithUnprefixedUrl(IHttpServerRequest request, string prefix)
        {
            var requestUriWithoutPrefix = request.Uri.RemovePrefix(prefix);

            return new HttpServerRequest(
                request.Headers,
                request.Method,
                requestUriWithoutPrefix,
                request.HttpVersion,
                request.ContentTypeCharset,
                request.AcceptCharsets,
                request.ContentLength,
                request.ContentType,
                request.AcceptEncodings,
                request.AcceptMediaTypes,
                request.Content,
                request.IsComplete);
        }

        public async Task<HttpServerResponse> HandleAsync(IHttpServerRequest request)
        {
            var unPrefixedRequest = CreateHttpRequestWithUnprefixedUrl(request, _urlPrefix);

            return await _routeHandler.HandleRequest(unPrefixedRequest);
        }

        public int CompareTo(RouteRegistration other)
        {
            return string.Compare(other._urlPrefix, _urlPrefix, StringComparison.OrdinalIgnoreCase);
        }
    }
}
