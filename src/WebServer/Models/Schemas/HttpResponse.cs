using Devkoes.Restup.WebServer.Models.Contracts;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    public struct HttpResponse : IHttpResponse
    {
        public string Response { get; }
        public byte[] RawResponse { get; }

        public HttpResponse(string response, byte[] rawResponse)
        {
            Response = response;
            RawResponse = rawResponse;
        }
    }
}
