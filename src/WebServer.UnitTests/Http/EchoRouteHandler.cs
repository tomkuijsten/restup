using System.Collections.Generic;
using System.Threading.Tasks;
using Devkoes.HttpMessage;
using Devkoes.Restup.WebServer.Models.Contracts;
using Devkoes.Restup.WebServer.UnitTests.TestHelpers;

namespace Devkoes.Restup.WebServer.UnitTests.Http
{
    public class EchoRouteHandler : IRouteHandler
    {
        private readonly List<IHttpServerRequest> _requests = new List<IHttpServerRequest>();

        internal IEnumerable<IHttpServerRequest> Requests => _requests;

        public Task<HttpServerResponse> HandleRequest(IHttpServerRequest request)
        {
            _requests.Add(request);
            return Task.FromResult(Utils.CreateOkHttpServerResponse(request.Content));
        }
    }
}