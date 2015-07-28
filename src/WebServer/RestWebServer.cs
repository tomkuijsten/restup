using Devkoes.Restup.WebServer.Helpers;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Models.Contracts;
using System.Collections.Generic;
using System;

namespace Devkoes.Restup.WebServer
{
    public class RestWebServer : HttpServer
    {
        private RestControllerRequestHandler _requestHandler;

        public RestWebServer() : base(8800)
        {
            _requestHandler = new RestControllerRequestHandler();
        }

        public void RegisterController<T>() where T : IRestController
        {
            _requestHandler.RegisterController<T>();
        }

        public override IRestResponse HandleRequest(string request)
        {
            string methodUrlAndHttpStatus = request.ToString().Split('\n')[0];
            string[] requestParts = methodUrlAndHttpStatus.Split(' ');
            string verb = requestParts[0];
            string uri = requestParts[1];
            
            return _requestHandler.HandleRequest(verb, uri);
        }
    }
}
