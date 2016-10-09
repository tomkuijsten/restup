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

        protected override object ExecuteAnonymousMethod(RestControllerMethodInfo info, RestServerRequest request, ParsedUri requestUri)
        {
            var instantiator = InstanceCreatorCache.Default.GetCreator(info.MethodInfo.DeclaringType);

            object[] parameters;
            try
            {
                parameters = info.GetParametersFromUri(requestUri).ToArray();
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
