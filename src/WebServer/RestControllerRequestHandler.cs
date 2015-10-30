using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Factories;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.InstanceCreators;
using Devkoes.Restup.WebServer.Models.Contracts;
using Devkoes.Restup.WebServer.Models.Schemas;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

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
            _restMethodCollection.AddRange(GetRestMethods<T>());

            InstanceCreatorCache.CacheCreator(typeof(T));
        }

        internal IEnumerable<RestMethodInfo> GetRestMethods<T>() where T : class
        {
            var restMethods = new List<RestMethodInfo>();

            var allPublicAsyncRestMethods = GetRestAsyncMethodDefinitions<T>();
            var allPublicRestMethods = GetRestMethodDefinitions<T>();

            foreach (var methodDef in allPublicRestMethods)
                restMethods.Add(new RestMethodInfo(methodDef));

            foreach (var methodDef in allPublicAsyncRestMethods)
                restMethods.Add(new RestMethodInfo(methodDef, true));

            return restMethods;
        }

        internal IEnumerable<MethodInfo> GetRestMethodDefinitions<T>()
        {
            var allPublicRestMethods =
               from m in typeof(T).GetRuntimeMethods()
               where
                   m.IsPublic &&
                   m.IsDefined(typeof(UriFormatAttribute)) &&
                   HasRestResponse(m)
               select m;

            return allPublicRestMethods.ToArray();
        }

        internal IEnumerable<MethodInfo> GetRestAsyncMethodDefinitions<T>()
        {
            var allPublicRestMethods =
               from m in typeof(T).GetRuntimeMethods()
               where
                   m.IsPublic &&
                   m.IsDefined(typeof(UriFormatAttribute)) &&
                   HasAsyncRestResponse(m)
               select m;

            return allPublicRestMethods.ToArray();
        }

        internal async Task<IRestResponse> HandleRequest(RestRequest req)
        {
            if (req.Verb == RestVerb.Unsupported)
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

            return await restMethodExecutor.ExecuteMethodAsync(restMethod, req);
        }

        private static bool HasAsyncRestResponse(MethodInfo m)
        {
            var isTask = m.ReturnType.GetTypeInfo().IsSubclassOf(typeof(Task));
            if (!isTask)
                return false;

            var genericArgs = m.ReturnType.GetGenericArguments();
            if (!genericArgs.Any())
                return false;

            return genericArgs[0].GetTypeInfo().ImplementedInterfaces.Contains(typeof(IRestResponse));
        }

        private static bool HasRestResponse(MethodInfo m)
        {
            return m.ReturnType.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IRestResponse));
        }
    }
}
