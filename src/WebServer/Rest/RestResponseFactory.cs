using Devkoes.Restup.WebServer.Models.Contracts;
using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Rest.Models.Contracts;

namespace Devkoes.Restup.WebServer.Rest
{
    internal class RestResponseFactory
    {
        private BadRequestResponse _badRequestResponse;

        internal RestResponseFactory()
        {
            _badRequestResponse = new BadRequestResponse();
        }

        internal IRestResponse CreateBadRequest()
        {
            return _badRequestResponse;
        }
    }
}
