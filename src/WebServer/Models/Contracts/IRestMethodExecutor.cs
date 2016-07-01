using System.Threading.Tasks;
using Restup.Webserver.Models.Schemas;
using Restup.Webserver.Rest;

namespace Restup.Webserver.Models.Contracts
{
    interface IRestMethodExecutor
    {
        Task<IRestResponse> ExecuteMethodAsync(RestControllerMethodInfo info, RestServerRequest request, ParsedUri requestUri);
    }
}
