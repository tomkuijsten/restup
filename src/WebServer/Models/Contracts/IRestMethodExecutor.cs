using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Rest;
using System.Threading.Tasks;
using Devkoes.Restup.WebServer.Rest.Models.Contracts;

namespace Devkoes.Restup.WebServer.Models.Contracts
{
    interface IRestMethodExecutor
    {
        Task<IRestResponse> ExecuteMethodAsync(RestControllerMethodInfo info, RestServerRequest request);
    }
}
