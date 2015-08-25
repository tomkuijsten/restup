using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Models.Contracts;
using Devkoes.Restup.WebServer.Builders;
using System.Linq;
using Devkoes.Restup.WebServer.Models.Schemas;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.IO;

namespace Devkoes.Restup.WebServer
{
    public class RestWebServer : HttpServer
    {
        private RestControllerRequestHandler _requestHandler;
        private RestRequestBuilder _restReqBuilder;
        private BodySerializer _bodySerializer;

        public RestWebServer() : base(8800)
        {
            _requestHandler = new RestControllerRequestHandler();
            _restReqBuilder = new RestRequestBuilder();
            _bodySerializer = new BodySerializer();
        }

        public void RegisterController<T>() where T : IRestController
        {
            _requestHandler.RegisterController<T>();
        }

        internal override IHttpResponse HandleRequest(string request)
        {
            var restRequest = _restReqBuilder.Build(request);

            var response = _requestHandler.HandleRequest(restRequest);

            string bodyString = _bodySerializer.ToBody(response.Data, restRequest);

            var httpResp = new HttpResponse(bodyString, restRequest.AcceptHeaders.First(), response.StatusCode);

            return httpResp;
        }
    }
}
