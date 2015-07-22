using Devkoes.Restup.WebServer.Helpers;
using Devkoes.Restup.WebServer.Models.Contracts;
using System.Collections.Generic;

namespace Devkoes.Restup.WebServer
{
    public class RestWebServer
    {
        private RestControllerRequestHandler _requestHandler;

        public RestWebServer(IEnumerable<IRestController> controllers)
        {
            _requestHandler = new RestControllerRequestHandler();
        }

        public void RegisterController<T>() where T : IRestController
        {
            _requestHandler.RegisterController<T>();
        }

        public IRestResponse HandleRequest(string request)
        {
            string methodUrlAndHttpStatus = request.ToString().Split('\n')[0];
            string[] requestParts = methodUrlAndHttpStatus.Split(' ');
            string verb = requestParts[0];
            string uri = requestParts[1];

            if(HttpHelpers.IsSupportedVerb(verb))
            {
                return null;
            }

            var restVerb = HttpHelpers.GetVerb(verb);
            
            return _requestHandler.HandleRequest(restVerb, uri);
        }
    }
}
