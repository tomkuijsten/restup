using Restup.Webserver.Models.Contracts;

namespace Restup.Webserver.Models.Schemas
{
    internal class StatusOnlyResponse : IRestResponse
    {
        public int StatusCode { get; private set; }

        internal StatusOnlyResponse(int statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
