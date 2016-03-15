using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Devkoes.Restup.WebServer.Models.Contracts;
using Devkoes.Restup.WebServer.Models.Schemas;
using WebServer.Rest.Models.Contracts;

namespace Devkoes.Restup.WebServer.Rest
{
    internal abstract class RestMethodExecutor : IRestMethodExecutor
    {
        public async Task<IRestResponse> ExecuteMethodAsync(RestControllerMethodInfo info, RestServerRequest request)
        {
            var methodInvokeResult = ExecuteAnonymousMethod(info, request);

            if (!info.IsAsync)
                return await Task.Run(() => (IRestResponse)methodInvokeResult);

            if (IsAsyncOperation(info))
                methodInvokeResult = ConvertToTask((dynamic)methodInvokeResult);

            return await(dynamic)methodInvokeResult;
        }

        private static Task<T> ConvertToTask<T>(IAsyncOperation<T> methodInvokeResult)
        {
            return methodInvokeResult.AsTask();
        }

        private static bool IsAsyncOperation(RestControllerMethodInfo info)
        {
            var returnType = info.MethodInfo.ReturnType;
            if (returnType.IsConstructedGenericType && returnType.GetGenericTypeDefinition() == typeof (IAsyncOperation<>))
                return true;

            return false;
        }

        protected abstract object ExecuteAnonymousMethod(RestControllerMethodInfo info, RestServerRequest request);
    }
}