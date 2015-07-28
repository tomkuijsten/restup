using Devkoes.Restup.WebServer.Models.Contracts;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    public class DefaultResponse : IRestResponse
    {
        public string Data { get; private set; }
        public int StatusCode { get; private set; }

        public DefaultResponse(string data, HttpResponseStatus code)
        {
            Data = data;
            StatusCode = (int)code;
        }
    }
}
