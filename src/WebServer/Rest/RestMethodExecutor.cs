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
            switch (info.ReturnTypeWrapper)
            {
                case RestControllerMethodInfo.TypeWrapper.None:
                    return await Task.FromResult((IRestResponse)methodInvokeResult);
                case RestControllerMethodInfo.TypeWrapper.AsyncOperation:
                    return await ConvertToTask((dynamic)methodInvokeResult);
                case RestControllerMethodInfo.TypeWrapper.Task:
                    return await (dynamic)methodInvokeResult;
            }

            throw new Exception($"ReturnTypeWrapper of type {info.ReturnTypeWrapper} not known.");
        }

        private static Task<T> ConvertToTask<T>(IAsyncOperation<T> methodInvokeResult)
        {
            return methodInvokeResult.AsTask();
        }

        protected abstract object ExecuteAnonymousMethod(RestControllerMethodInfo info, RestServerRequest request);
    }
}