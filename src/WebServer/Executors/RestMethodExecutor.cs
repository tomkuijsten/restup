using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Models.Contracts;
using System.Linq;
using Devkoes.Restup.WebServer.InstanceCreators;

namespace Devkoes.Restup.WebServer.Executors
{
    internal class RestMethodExecutor : IRestMethodExecutor
    {
        public IRestResponse ExecuteMethod(RestMethodInfo info, RestRequest request)
        {
            var instantiator = InstanceCreatorCache.GetCreator(info.MethodInfo.DeclaringType);

            return (IRestResponse)info.MethodInfo.Invoke(
                    instantiator.Create(info.MethodInfo.DeclaringType),
                    info.GetParametersFromUri(request.Uri).ToArray());
        }
    }
}
