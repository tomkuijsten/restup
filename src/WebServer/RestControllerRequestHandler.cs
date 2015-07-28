using Devkoes.Restup.WebServer.Models.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Helpers;
using System.Net;

namespace Devkoes.Restup.WebServer
{
    public class RestControllerRequestHandler
    {
        private List<RestMethodInfo> _restMethodCollection;
        private DefaultResponse _unsupportedVerbResponse;
        private DefaultResponse _invalidUriResponse;

        public RestControllerRequestHandler()
        {
            _restMethodCollection = new List<RestMethodInfo>();

            _unsupportedVerbResponse = new DefaultResponse("Verb not supported", HttpStatusCode.BadRequest);
            _invalidUriResponse = new DefaultResponse("No REST controller for uri found", HttpStatusCode.BadRequest);
        }

        public void RegisterController<T>() where T : IRestController
        {
            var allPublicRestMethods =
                from m in typeof(T).GetRuntimeMethods()
                where
                    m.IsPublic &&
                    m.ReturnType.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IRestResponse)) &&
                    m.IsDefined(typeof(UriFormatAttribute))
                select m;

            // TODO: check if uriformat is unique

            foreach (var methodDef in allPublicRestMethods)
            {
                _restMethodCollection.Add(new RestMethodInfo(methodDef));
            }
        }

        public IRestResponse HandleRequest(string verb, string uri)
        {
            if (!HttpHelpers.IsSupportedVerb(verb))
            {
                return _unsupportedVerbResponse;
            }

            var restVerb = HttpHelpers.GetVerb(verb);

            var restMethod = _restMethodCollection.SingleOrDefault(r => r.Match(restVerb, uri));
            if (restMethod == null)
            {
                return _invalidUriResponse;
            }

            return restMethod.ExecuteMethod(uri);
        }
    }
}
