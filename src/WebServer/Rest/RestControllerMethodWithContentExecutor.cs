using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.InstanceCreators;
using Devkoes.Restup.WebServer.Models.Contracts;
using Devkoes.Restup.WebServer.Models.Schemas;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebServer.Rest.Models.Contracts;

namespace Devkoes.Restup.WebServer.Rest
{
    internal class RestControllerMethodWithContentExecutor : IRestMethodExecutor
    {
        private ContentSerializer _contentSerializer;
        private RestResponseFactory _responseFactory;

        public RestControllerMethodWithContentExecutor()
        {
            _contentSerializer = new ContentSerializer();
            _responseFactory = new RestResponseFactory();
        }

        public async Task<IRestResponse> ExecuteMethodAsync(RestControllerMethodInfo info, RestServerRequest request)
        {
            var methodInvokeResult = ExecuteAnonymousMethod(info, request);

            if (!info.IsAsync)
                return await Task.Run(() => (IRestResponse)methodInvokeResult);

            return await (dynamic)methodInvokeResult;
        }

        private object ExecuteAnonymousMethod(RestControllerMethodInfo info, RestServerRequest request)
        {
            var instantiator = InstanceCreatorCache.Default.GetCreator(info.MethodInfo.DeclaringType);

            object contentObj = null;
            try
            {
                if (request.HttpServerRequest.Content != null)
                {
                    contentObj = _contentSerializer.FromContent(
                        request.ContentEncoding.GetString(request.HttpServerRequest.Content),
                        request.ContentMediaType,
                        info.ContentParameterType);
                }
            }
            catch (JsonReaderException)
            {
                return _responseFactory.CreateBadRequest();
            }
            catch (InvalidOperationException)
            {
                return _responseFactory.CreateBadRequest();
            }

            object[] parameters = null;
            try
            {
                parameters = info.GetParametersFromUri(request.HttpServerRequest.Uri).Concat(new[] { contentObj }).ToArray();
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
