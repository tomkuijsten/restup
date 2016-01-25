using Devkoes.HttpMessage;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.InstanceCreators;
using Devkoes.Restup.WebServer.Models.Contracts;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IRestResponse> ExecuteMethodAsync(RestControllerMethodInfo info, HttpServerRequest request)
        {
            var methodInvokeResult = ExecuteAnonymousMethod(info, request);

            if (!info.IsAsync)
                return await Task.Run(() => (IRestResponse)methodInvokeResult);

            return await (dynamic)methodInvokeResult;
        }

        private object ExecuteAnonymousMethod(RestControllerMethodInfo info, HttpServerRequest request)
        {
            var instantiator = InstanceCreatorCache.Default.GetCreator(info.MethodInfo.DeclaringType);

            object contentObj = null;
            try
            {
                contentObj = GetContentObject(info, request);
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
                parameters = info.GetParametersFromUri(request.Uri).Concat(new[] { contentObj }).ToArray();
            }
            catch (FormatException)
            {
                return _responseFactory.CreateBadRequest();
            }

            return info.MethodInfo.Invoke(
                    instantiator.Create(info.MethodInfo.DeclaringType, info.ControllerConstructorArgs()),
                    parameters);
        }

        private object GetContentObject(RestControllerMethodInfo info, HttpServerRequest request)
        {
            var contentType = request.ContentType ?? Configuration.Default.ContentType;
            var charset = request.ContentTypeCharset ?? HttpDefaults.Default.GetDefaultCharset(contentType);
            string content = _contentSerializer.GetContentString(charset, request.Content);
            object contentObj = _contentSerializer.FromContent(content, contentType, info.ContentParameterType);

            return contentObj;
        }
    }
}
