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
        private readonly ContentSerializer _contentSerializer;
        private readonly RestResponseFactory _responseFactory;

        public RestControllerMethodWithContentExecutor()
        {
            _contentSerializer = new ContentSerializer();
            _responseFactory = new RestResponseFactory();
        }

        protected override object ExecuteAnonymousMethod(RestControllerMethodInfo info, RestServerRequest request, ParsedUri requestUri)
        {
            var instantiator = InstanceCreatorCache.Default.GetCreator(info.MethodInfo.DeclaringType);

            object contentObj = null;
            try
            {
                if (request.HttpServerRequest.Content != null)
                {
                    contentObj = _contentSerializer.FromContent(
                        request.HttpServerRequest.Content,
                        request.ContentMediaType,
                        info.ContentParameterType,
                        request.ContentEncoding);
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
                parameters = info.GetParametersFromUri(requestUri).Concat(new[] { contentObj }).ToArray();
            }
            catch (FormatException)
            {
                return _responseFactory.CreateBadRequest();
            }

            return info.MethodInfo.Invoke(
                    instantiator.Create(info.ControllerConstructor, info.ControllerConstructorArgs()),
                    parameters);
        }
    }
}
