using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Models.Contracts;
using System.Linq;
using Devkoes.Restup.WebServer.InstanceCreators;
using System.Collections.Generic;
using System;
using Devkoes.Restup.WebServer.Factories;

namespace Devkoes.Restup.WebServer.Executors
{
    internal class RestMethodExecutor : IRestMethodExecutor
    {
        private RestResponseFactory _responseFactory;

        public RestMethodExecutor()
        {
            _responseFactory = new RestResponseFactory();
        }

        public IRestResponse ExecuteMethod(RestMethodInfo info, RestRequest request)
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

            return (IRestResponse)info.MethodInfo.Invoke(
                    instantiator.Create(info.MethodInfo.DeclaringType),
                    parameters);
        }
    }
}
