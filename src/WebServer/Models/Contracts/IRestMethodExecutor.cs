using Restup.Webserver.Models.Schemas;
using Restup.Webserver.Rest;
using System.Threading.Tasks;

namespace Restup.Webserver.Models.Contracts
{
    interface IRestMethodExecutor
    {
        Task<IRestResponse> ExecuteMethodAsync(RestControllerMethodInfo info, RestServerRequest request);
    }
}
