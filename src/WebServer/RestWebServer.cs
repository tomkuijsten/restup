using Devkoes.Restup.WebServer.Converters;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Http.RequestFactory;
using Devkoes.Restup.WebServer.Models.Contracts;
using System.Threading.Tasks;

namespace Devkoes.Restup.WebServer
{
    public class RestWebServer : HttpServer
    {
        private RestControllerRequestHandler _requestHandler;
        private RestResponseToHttpResponseConverter _restToHttpConverter;

        public RestWebServer(int port, string urlPrefix) : base(port)
        {
            var fixedFormatUrlPrefix = FixPrefixUrlFormatting(urlPrefix);

            _requestHandler = new RestControllerRequestHandler(fixedFormatUrlPrefix);
            _restToHttpConverter = new RestResponseToHttpResponseConverter();
        }

        /// <summary>
        /// The prefix will always be formatted as "/prefix"
        /// </summary>
        private string FixPrefixUrlFormatting(string urlPrefix)
        {
            var cleanUrl = UriHelper.RemovePreAndPostSlash(urlPrefix);

            return string.IsNullOrWhiteSpace(cleanUrl) ? string.Empty : "/" + cleanUrl;
        }

        public RestWebServer(int port) : this(port, null) { }

        public RestWebServer() : this(8800, null) { }

        public void RegisterController<T>() where T : class
        {
            _requestHandler.RegisterController<T>();
        }

        internal override async Task<IHttpResponse> HandleRequest(HttpRequest request)
        {
            var restResponse = await _requestHandler.HandleRequest(request);

            var httpResponse = restResponse.Visit(_restToHttpConverter, request);

            return httpResponse;
        }
    }
}
