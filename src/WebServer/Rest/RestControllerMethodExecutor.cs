using Restup.Webserver.InstanceCreators;
using Restup.Webserver.Models.Schemas;
using System;
using System.Linq;

namespace Restup.Webserver.Rest
{
    internal class RestControllerMethodExecutor : RestMethodExecutor
    {
        private readonly RestResponseFactory _responseFactory;

        public RestControllerMethodExecutor()
        {
            _responseFactory = new RestResponseFactory();
        }

        protected override object ExecuteAnonymousMethod(RestControllerMethodInfo info, RestServerRequest request)
        {
            var instantiator = InstanceCreatorCache.Default.GetCreator(info.MethodInfo.DeclaringType);

            object[] parameters = null;
            try
            {
                parameters = info.GetParametersFromUri(request.HttpServerRequest.Uri).ToArray();
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
