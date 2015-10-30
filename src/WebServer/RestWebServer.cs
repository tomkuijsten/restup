using Devkoes.Restup.WebServer.Builders;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Models.Contracts;
using Devkoes.Restup.WebServer.Visitors;
using System.Threading.Tasks;

namespace Devkoes.Restup.WebServer
{
    public class RestWebServer : HttpServer
    {
        private RestControllerRequestHandler _requestHandler;
        private RestRequestBuilder _restReqBuilder;

        public RestWebServer(int port) : base(port)
        {
            _requestHandler = new RestControllerRequestHandler();
            _restReqBuilder = new RestRequestBuilder();
        }

        public RestWebServer() : this(8800) { }

        public void RegisterController<T>() where T : class
        {
            _requestHandler.RegisterController<T>();
        }

        internal override async Task<IHttpResponse> HandleRequest(string request)
        {
            var restRequest = _restReqBuilder.Build(request);

            var restResponse = await _requestHandler.HandleRequest(restRequest);

            var responseVisitor = new RestResponseVisitor(restRequest);
            restResponse.Accept(responseVisitor);

            return responseVisitor.HttpResponse;
        }
    }
}
