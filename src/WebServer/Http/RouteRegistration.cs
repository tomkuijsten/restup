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
            _urlPrefix = urlPrefix.FormatRelativeUri();
            _routeHandler = routeHandler;
        }

        public bool Match(IHttpServerRequest request)
        {
			if(request.Uri == null)
			{
				return false;
			}
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
                request.IsComplete,
                request.AccessControlRequestMethod,
                request.AccessControlRequestHeaders,
                request.Origin);
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

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((RouteRegistration)obj);
        }

        protected bool Equals(RouteRegistration other)
        {
            return string.Equals(_urlPrefix, other._urlPrefix);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return _urlPrefix?.GetHashCode() ?? 0;
            }
        }
    }
}
