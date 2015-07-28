using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Models.Contracts;
using Devkoes.Restup.WebServer.Builders;

namespace Devkoes.Restup.WebServer
{
    public class RestWebServer : HttpServer
    {
        private RestControllerRequestHandler _requestHandler;
        private RestRequestBuilder _restReqBuilder;

        public RestWebServer() : base(8800)
        {
            _requestHandler = new RestControllerRequestHandler();
            _restReqBuilder = new RestRequestBuilder();
        }

        public void RegisterController<T>() where T : IRestController
        {
            _requestHandler.RegisterController<T>();
        }

        public override IRestResponse HandleRequest(string request)
        {
            var restRequest = _restReqBuilder.Build(request);

            return _requestHandler.HandleRequest(restRequest.Verb, restRequest.Uri);
        }
    }
}
