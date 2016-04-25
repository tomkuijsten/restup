using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;

namespace Restup.Webserver.Rest
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
