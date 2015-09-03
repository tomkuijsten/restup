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
        private RestResponseFactory _responseFactory;
        private RestMethodExecutorFactory _methodExecuteFactory;

        internal RestControllerRequestHandler()
        {
            _restMethodCollection = new List<RestMethodInfo>();
            _responseFactory = new RestResponseFactory();
            _methodExecuteFactory = new RestMethodExecutorFactory();
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
                return _responseFactory.CreateBadRequest();
            }
            
            var restMethods = _restMethodCollection.Where(r => r.Match(req.Uri));
            if (!restMethods.Any())
            {
                return _responseFactory.CreateBadRequest();
            }

            var restMethod = restMethods.SingleOrDefault(r => r.Verb == req.Verb);
            if (restMethod == null)
            {
                return new MethodNotAllowedResponse(restMethods.Select(r => r.Verb));
            }

            var restMethodExecutor = _methodExecuteFactory.Create(restMethod);

            return restMethodExecutor.ExecuteMethod(restMethod, req);
        }
    }
}
