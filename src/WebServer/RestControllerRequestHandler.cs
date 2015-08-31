using Devkoes.Restup.WebServer.Models.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Http;
using System.Net;
using Devkoes.Restup.WebServer.Factories;

namespace Devkoes.Restup.WebServer
{
    internal class RestControllerRequestHandler
    {
        private List<RestMethodInfo> _restMethodCollection;
        private DefaultResponse _unsupportedVerbResponse;
        private DefaultResponse _invalidUriResponse;
        private RestMethodExecutorFactory _methodExecuteFactory;

        internal RestControllerRequestHandler()
        {
            _restMethodCollection = new List<RestMethodInfo>();
            _methodExecuteFactory = new RestMethodExecutorFactory();

            _unsupportedVerbResponse = new DefaultResponse("Verb not supported", DefaultResponse.ResponseStatus.BadRequest);
            _invalidUriResponse = new DefaultResponse("No REST controller for uri found", DefaultResponse.ResponseStatus.BadRequest);
        }

        internal void RegisterController<T>() where T : class
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

        internal IRestResponse HandleRequest(RestRequest req)
        {
            if(req.Verb == RestVerb.Unsupported)
            {
                return _unsupportedVerbResponse;
            }
            
            var restMethod = _restMethodCollection.SingleOrDefault(r => r.Match(req.Verb, req.Uri));
            if (restMethod == null)
            {
                return _invalidUriResponse;
            }

            var restMethodExecutor = _methodExecuteFactory.Create(restMethod);

            return restMethodExecutor.ExecuteMethod(restMethod, req);
        }
    }
}
