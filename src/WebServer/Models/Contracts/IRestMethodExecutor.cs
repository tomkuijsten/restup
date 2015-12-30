using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Http.RequestFactory;
using System.Threading.Tasks;

namespace Devkoes.Restup.WebServer.Models.Contracts
{
    internal interface IRestMethodExecutor
    {
        Task<IRestResponse> ExecuteMethodAsync(RestMethodInfo info, HttpRequest request);
    }
}
