using Devkoes.Restup.WebServer.Factories;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.InstanceCreators;
using Devkoes.Restup.WebServer.Models.Contracts;
using Devkoes.Restup.WebServer.Models.Schemas;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Devkoes.Restup.WebServer.Executors
{
    internal class RestMethodExecutor : IRestMethodExecutor
    {
        private RestResponseFactory _responseFactory;

        public RestMethodExecutor()
        {
            _responseFactory = new RestResponseFactory();
        }

        public async Task<IRestResponse> ExecuteMethodAsync(RestMethodInfo info, RestRequest request)
        {
            var methodInvokeResult = ExecuteAnonymousMethod(info, request);

            if (!info.IsAsync)
                return (IRestResponse)methodInvokeResult;

            return await (dynamic)methodInvokeResult;
        }

        private object ExecuteAnonymousMethod(RestMethodInfo info, RestRequest request)
        {
            var instantiator = InstanceCreatorCache.GetCreator(info.MethodInfo.DeclaringType);

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
                    instantiator.Create(info.MethodInfo.DeclaringType),
                    parameters);
        }
    }
}
