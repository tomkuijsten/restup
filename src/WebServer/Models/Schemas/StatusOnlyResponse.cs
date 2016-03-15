using WebServer.Rest.Models.Contracts;

namespace Devkoes.Restup.WebServer.Models.Schemas
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
