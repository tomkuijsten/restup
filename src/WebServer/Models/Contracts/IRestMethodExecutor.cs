using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Models.Schemas;

namespace Devkoes.Restup.WebServer.Models.Contracts
{
    internal interface IRestMethodExecutor
    {
        IRestResponse ExecuteMethod(RestMethodInfo info, RestRequest request);
    }
}
