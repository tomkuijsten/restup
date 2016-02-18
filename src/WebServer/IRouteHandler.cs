using System.Threading.Tasks;
using Devkoes.HttpMessage;

namespace Devkoes.Restup.WebServer
{
    public interface IRouteHandler
    {
        Task<HttpServerResponse> HandleRequest(IHttpServerRequest request);
    }
}