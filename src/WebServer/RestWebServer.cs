using Devkoes.Restup.WebServer.Builders;
using Devkoes.Restup.WebServer.Converters;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Models.Contracts;
using System.Threading.Tasks;

namespace Devkoes.Restup.WebServer
{
    public class RestWebServer : HttpServer
    {
        private RestControllerRequestHandler _requestHandler;
        private RestRequestBuilder _restReqBuilder;
        private RestResponseToHttpResponseConverter _restToHttpConverter;

        public RestWebServer(int port, string urlPrefix) : base(port)
        {
            _requestHandler = new RestControllerRequestHandler(urlPrefix);
            _restReqBuilder = new RestRequestBuilder();
            _restToHttpConverter = new RestResponseToHttpResponseConverter();
        }

        public RestWebServer(int port) : this(port, null) { }

        public RestWebServer() : this(8800, null) { }

        public void RegisterController<T>() where T : class
        {
            _requestHandler.RegisterController<T>();
        }

        internal override async Task<IHttpResponse> HandleRequest(string request)
        {
            var restRequest = _restReqBuilder.Build(request);

            var restResponse = await _requestHandler.HandleRequest(restRequest);

            var httpResponse = restResponse.Visit(_restToHttpConverter, restRequest);

            return httpResponse;
        }
    }
}
