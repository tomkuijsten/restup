using Devkoes.HttpMessage;
using Devkoes.Restup.WebServer.Models.Contracts;
using System;
using System.Threading.Tasks;

namespace Devkoes.Restup.WebServer.Rest
{
    public class RestRouteHandler : IRouteHandler
    {
        private readonly RestControllerRequestHandler _requestHandler;
        private readonly RestToHttpResponseConverter _restToHttpConverter;
        private readonly RestServerRequestFactory _restServerRequestFactory;

        public RestRouteHandler()
        {
            _restServerRequestFactory = new RestServerRequestFactory();
            _requestHandler = new RestControllerRequestHandler();
            _restToHttpConverter = new RestToHttpResponseConverter();
        }

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

        public async Task<HttpServerResponse> HandleRequest(IHttpServerRequest request)
        {
            var restServerRequest = _restServerRequestFactory.Create(request);

            var restResponse = await _requestHandler.HandleRequest(restServerRequest);

            var httpResponse = restResponse.Visit(_restToHttpConverter, restServerRequest);

            return httpResponse;
        }
    }
}