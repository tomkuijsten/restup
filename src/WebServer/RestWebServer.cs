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

        public RestWebServer() : base(8800)
        {
            _requestHandler = new RestControllerRequestHandler();
            _restReqBuilder = new RestRequestBuilder();
        }

        public void RegisterController<T>() where T : IRestController
        {
            _requestHandler.RegisterController<T>();
        }

        internal override IHttpResponse HandleRequest(string request)
        {
            var restRequest = _restReqBuilder.Build(request);

            var response = _requestHandler.HandleRequest(restRequest.Verb, restRequest.Uri);

            string bodyString = null;
            if (response.Data != null)
            {
                if (restRequest.AcceptHeaders.First() == AcceptMediaType.JSON)
                {
                    bodyString = JsonConvert.SerializeObject(response.Data);
                }
                else if(restRequest.AcceptHeaders.First() == AcceptMediaType.XML)
                {
                    bodyString = SerializeObject(response.Data);
                }
            }

            var httpResp = new HttpResponse(bodyString, restRequest.AcceptHeaders.First(), response.StatusCode);

            return httpResp;
        }

        private static string SerializeObject(object toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }
    }
}
