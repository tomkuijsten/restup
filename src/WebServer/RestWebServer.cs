using Devkoes.HttpMessage;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Rest;
using System;
using System.Threading.Tasks;

namespace Devkoes.Restup.WebServer
{
    public class RestWebServer : HttpServer
    {
        private RestControllerRequestHandler _requestHandler;
        private RestToHttpResponseConverter _restToHttpConverter;
        private RestServerRequestFactory _restServerRequestFactory;
        private readonly string fixedFormatUrlPrefix;

        public RestWebServer(int port, string urlPrefix) : base(port)
        {
            fixedFormatUrlPrefix = urlPrefix.FormatRelativeUri();

            _restServerRequestFactory = new RestServerRequestFactory();
            _requestHandler = new RestControllerRequestHandler();
            _restToHttpConverter = new RestToHttpResponseConverter();
        }

        public RestWebServer(int port) : this(port, null) { }

        public RestWebServer() : this(8800, null) { }

        public void RegisterController<T>() where T : class
        {
            _requestHandler.RegisterController<T>();
        }

        public void RegisterController<T>(params object[] args) where T : class
        {
            _requestHandler.RegisterController<T>(() => args);
        }

        public void RegisterController<T>(Func<object[]> args) where T : class
        {
            _requestHandler.RegisterController<T>(args);
        }

        internal override async Task<HttpServerResponse> HandleRequest(IHttpServerRequest request)
        {
            var unprefixedRequest = CreateHttpRequestWithUnprefixedUrl(request, fixedFormatUrlPrefix);

            var restServerRequest = _restServerRequestFactory.Create(unprefixedRequest);

            var restResponse = await _requestHandler.HandleRequest(restServerRequest);

            var httpResponse = restResponse.Visit(_restToHttpConverter, restServerRequest);

            return httpResponse;
        }

        private static IHttpServerRequest CreateHttpRequestWithUnprefixedUrl(IHttpServerRequest request, string prefix)
        {
            return new HttpServerRequest(request.Headers, request.Method, RemovePrefix(request.Uri, prefix), request.HttpVersion,
                request.ContentTypeCharset, request.AcceptCharsets, request.ContentLength, request.ContentType,
                request.AcceptMediaTypes, request.Content, request.IsComplete);
        }

        private static Uri RemovePrefix(Uri uri, string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                return uri;

            var uriToString = uri.ToString();
            if (uriToString.StartsWith(prefix))
                uriToString = uriToString.Remove(0, prefix.Length);

            return new Uri(uriToString, UriKind.Relative);
        }
    }
}
