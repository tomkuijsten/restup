using Devkoes.Restup.WebServer.Models.Contracts;
using Devkoes.Restup.WebServer.Models.Schemas;

namespace Devkoes.Restup.WebServer.Factories
{
    public class RestResponseFactory
    {
        private BadRequestResponse _badRequestResponse;

        public RestResponseFactory()
        {
            _badRequestResponse = new BadRequestResponse();
        }

        public IRestResponse CreateBadRequest()
        {
            return _badRequestResponse;
        }
    }
}
