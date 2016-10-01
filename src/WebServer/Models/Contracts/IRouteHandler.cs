using Restup.HttpMessage;
using System.Threading.Tasks;

namespace Restup.Webserver.Models.Contracts
{
    public interface IRouteHandler
    {
        Task<HttpServerResponse> HandleRequest(IHttpServerRequest request);
    }
}