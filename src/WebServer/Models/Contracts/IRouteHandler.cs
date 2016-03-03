using Devkoes.HttpMessage;
using System.Threading.Tasks;

namespace Devkoes.Restup.WebServer.Models.Contracts
{
    public interface IRouteHandler
    {
        Task<HttpServerResponse> HandleRequest(IHttpServerRequest request);
    }
}