using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Http;
using System.Linq;
using Devkoes.Restup.WebServer.Models.Contracts;
using Devkoes.Restup.WebServer.InstanceCreators;
using System.Collections.Generic;
using System;
using Devkoes.Restup.WebServer.Factories;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Devkoes.Restup.WebServer.Executors
{
    internal class RestMethodWithBodyExecutor : IRestMethodExecutor
    {
        private BodySerializer _bodySerializer;
        private RestResponseFactory _responseFactory;

        public RestMethodWithBodyExecutor()
        {
            _bodySerializer = new BodySerializer();
            _responseFactory = new RestResponseFactory();
        }

        public async Task<IRestResponse> ExecuteMethodAsync(RestMethodInfo info, RestRequest request)
        {
            var methodInvokeResult = ExecuteAnonymousMethod(info, request);

            if (!info.IsAsync)
                return await Task.Run(() => (IRestResponse)methodInvokeResult);

            return await (dynamic)methodInvokeResult;
        }

        private object ExecuteAnonymousMethod(RestMethodInfo info, RestRequest request)
        {
            var instantiator = InstanceCreatorCache.GetCreator(info.MethodInfo.DeclaringType);

            object bodyObj = null;
            try
            {
                bodyObj = _bodySerializer.FromBody(request.Body, request.BodyMediaType, info.BodyParameterType);
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
                parameters = info.GetParametersFromUri(request.Uri).Union(new[] { bodyObj }).ToArray();
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
