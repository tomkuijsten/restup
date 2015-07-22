using Devkoes.Restup.WebServer.Models.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Http;

namespace Devkoes.Restup.WebServer
{
    public class RestControllerRequestHandler
    {
        private List<RestMethodInfo> _restMethodCollection;

        public RestControllerRequestHandler()
        {
            _restMethodCollection = new List<RestMethodInfo>();
        }

        public void RegisterController<T>() where T : IRestController
        {
            var allPublicRestMethods =
                from m in typeof(T).GetRuntimeMethods()
                where
                    m.IsPublic &&
                    m.ReturnType.GetTypeInfo().IsSubclassOf(typeof(IRestResponse)) &&
                    m.IsDefined(typeof(UriFormatAttribute))
                select m;

            foreach (var methodDef in allPublicRestMethods)
            {
                var uriFormat = methodDef.GetCustomAttribute<UriFormatAttribute>();
                _restMethodCollection.Add(new RestMethodInfo(methodDef, uriFormat.UriFormat));
            }

            // TODO: check if uriformat is unique
        }

        public IRestResponse HandleRequest(RestVerb verb, string uri)
        {
            var restMethod = _restMethodCollection.SingleOrDefault(r => r.Match(verb, uri));
            if (restMethod != null)
            {
                return (IRestResponse)restMethod.MethodInfo.Invoke(restMethod.Instantiator.Create(), restMethod.GetParametersFromUri(uri));
            }

            return null;
        }
    }
}
