using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Http;
using System.Linq;
using Devkoes.Restup.WebServer.Models.Contracts;
using Devkoes.Restup.WebServer.InstanceCreators;

namespace Devkoes.Restup.WebServer.Executors
{
    internal class RestMethodWithBodyExecutor : IRestMethodExecutor
    {
        private BodySerializer _bodySerializer;

        public RestMethodWithBodyExecutor()
        {
            _bodySerializer = new BodySerializer();
        }

        public IRestResponse ExecuteMethod(RestMethodInfo info, RestRequest request)
        {
            var instantiator = InstanceCreatorCache.GetCreator(info.MethodInfo.DeclaringType);

            var bodyObj = _bodySerializer.FromBody(request.Body, request.BodyMediaType, info.BodyParameterType);

            var parameters = info.GetParametersFromUri(request.Uri).Union(new[] { bodyObj });

            return (IRestResponse)info.MethodInfo.Invoke(
                    instantiator.Create(info.MethodInfo.DeclaringType),
                    parameters.ToArray());
        }
    }
}
