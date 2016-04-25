using Newtonsoft.Json;
using Restup.Webserver.Http;
using Restup.Webserver.InstanceCreators;
using Restup.Webserver.Models.Schemas;
using System;
using System.Linq;

namespace Restup.Webserver.Rest
{
    internal class RestControllerMethodWithContentExecutor : RestMethodExecutor
    {
        private ContentSerializer _contentSerializer;
        private RestResponseFactory _responseFactory;

        public RestControllerMethodWithContentExecutor()
        {
            _contentSerializer = new ContentSerializer();
            _responseFactory = new RestResponseFactory();
        }

        protected override object ExecuteAnonymousMethod(RestControllerMethodInfo info, RestServerRequest request)
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
