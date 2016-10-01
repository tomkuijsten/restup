using Restup.HttpMessage;
using Restup.Webserver.Models.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restup.Webserver.UnitTests.TestHelpers
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