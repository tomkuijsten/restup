using Restup.HttpMessage.Models.Schemas;
using Restup.Webserver.Attributes;
using Restup.Webserver.InstanceCreators;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Restup.Webserver.Rest
{
    internal class RestControllerRequestHandler
    {
        private ImmutableArray<RestControllerMethodInfo> _restMethodCollection;
        private readonly RestResponseFactory _responseFactory;
        private readonly RestControllerMethodExecutorFactory _methodExecuteFactory;
        private readonly UriParser _uriParser;
        private readonly RestControllerMethodInfoValidator _restControllerMethodInfoValidator;

        internal RestControllerRequestHandler()
        {
            _restMethodCollection = ImmutableArray<RestControllerMethodInfo>.Empty;
            _responseFactory = new RestResponseFactory();
            _methodExecuteFactory = new RestControllerMethodExecutorFactory();
            _uriParser = new UriParser();
            _restControllerMethodInfoValidator = new RestControllerMethodInfoValidator();
        }

        internal void RegisterController<T>() where T : class
        {
            RegisterController<T>(() => Enumerable.Empty<object>().ToArray());
        }

        internal void RegisterController<T>(Func<object[]> constructorArgs) where T : class
        {
            var restControllerMethodInfos = GetRestMethods<T>(constructorArgs);
            AddRestMethods<T>(restControllerMethodInfos);
        }

        private void AddRestMethods<T>(IEnumerable<RestControllerMethodInfo> restControllerMethodInfos) where T : class
        {
            var newControllerMethodInfos = restControllerMethodInfos.ToArray();

            _restControllerMethodInfoValidator.Validate<T>(_restMethodCollection, newControllerMethodInfos);

            _restMethodCollection = _restMethodCollection.Concat(newControllerMethodInfos)
                .OrderByDescending(x => x.MethodInfo.GetParameters().Count())
                .ToImmutableArray();

            InstanceCreatorCache.Default.CacheCreator(typeof(T));
        }

        internal IEnumerable<RestControllerMethodInfo> GetRestMethods<T>(Func<object[]> constructorArgs) where T : class
        {
            var possibleValidRestMethods = (from m in typeof(T).GetRuntimeMethods()
                                            where m.IsPublic &&
                                                  m.IsDefined(typeof(UriFormatAttribute))
                                            select m).ToList();

            foreach (var restMethod in possibleValidRestMethods)
            {
                if (HasRestResponse(restMethod))
                    yield return new RestControllerMethodInfo(restMethod, constructorArgs, RestControllerMethodInfo.TypeWrapper.None);
                else if (HasAsyncRestResponse(restMethod, typeof(Task<>)))
                    yield return new RestControllerMethodInfo(restMethod, constructorArgs, RestControllerMethodInfo.TypeWrapper.Task);
                else if (HasAsyncRestResponse(restMethod, typeof(IAsyncOperation<>)))
                    yield return new RestControllerMethodInfo(restMethod, constructorArgs, RestControllerMethodInfo.TypeWrapper.AsyncOperation);
            }
        }

        private static bool HasRestResponse(MethodInfo m)
        {
            return m.ReturnType.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IRestResponse));
        }

        private static bool HasAsyncRestResponse(MethodInfo m, Type type)
        {
            if (!m.ReturnType.IsConstructedGenericType)
                return false;

            var genericTypeDefinition = m.ReturnType.GetGenericTypeDefinition();
            var isAsync = genericTypeDefinition == type;
            if (!isAsync)
                return false;

            var genericArgs = m.ReturnType.GetGenericArguments();
            if (!genericArgs.Any())
            {
                return false;
            }

            return genericArgs[0].GetTypeInfo().ImplementedInterfaces.Contains(typeof(IRestResponse));
        }

        internal async Task<IRestResponse> HandleRequestAsync(RestServerRequest req)
        {
            if (!req.HttpServerRequest.IsComplete ||
                req.HttpServerRequest.Method == HttpMethod.Unsupported)
            {
                return _responseFactory.CreateBadRequest();
            }

            ParsedUri parsedUri;
            var incomingUriAsString = req.HttpServerRequest.Uri.ToRelativeString();
            if (!_uriParser.TryParse(incomingUriAsString, out parsedUri))
            {
                throw new Exception($"Could not parse uri: {incomingUriAsString}");
            }

            var restMethods = _restMethodCollection.Where(r => r.Match(parsedUri)).ToList();
            if (!restMethods.Any())
            {
                return _responseFactory.CreateBadRequest();
            }

            var restMethod = restMethods.FirstOrDefault(r => r.Verb == req.HttpServerRequest.Method);
            if (restMethod == null)
            {
                return new MethodNotAllowedResponse(restMethods.Select(r => r.Verb));
            }

            var restMethodExecutor = _methodExecuteFactory.Create(restMethod);

            try
            {
                return await restMethodExecutor.ExecuteMethodAsync(restMethod, req, parsedUri);
            }
            catch
            {
                return _responseFactory.CreateBadRequest();
            }
        }
    }
}
