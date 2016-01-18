using Devkoes.HttpMessage;
using Devkoes.Restup.WebServer.InstanceCreators;
using Devkoes.Restup.WebServer.Models.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Devkoes.Restup.WebServer.Rest
{
    internal class RestControllerMethodExecutor : IRestMethodExecutor
    {
        private RestResponseFactory _responseFactory;

        public RestControllerMethodExecutor()
        {
            _responseFactory = new RestResponseFactory();
        }

        public async Task<IRestResponse> ExecuteMethodAsync(RestControllerMethodInfo info, HttpServerRequest request)
        {
            var methodInvokeResult = ExecuteAnonymousMethod(info, request);

            if (!info.IsAsync)
                return (IRestResponse)methodInvokeResult;

            return await (dynamic)methodInvokeResult;
        }

        private object ExecuteAnonymousMethod(RestControllerMethodInfo info, HttpServerRequest request)
        {
            var instantiator = InstanceCreatorCache.Default.GetCreator(info.MethodInfo.DeclaringType);

            object[] parameters = null;
            try
            {
                parameters = info.GetParametersFromUri(request.Uri).ToArray();
            }
            catch (FormatException)
            {
                return _responseFactory.CreateBadRequest();
            }

            return info.MethodInfo.Invoke(
                    instantiator.Create(info.MethodInfo.DeclaringType, info.ControllerConstructorArgs()),
                    parameters);
        }
    }
}
