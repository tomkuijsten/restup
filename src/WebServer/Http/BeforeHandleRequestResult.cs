using Restup.HttpMessage;

namespace Restup.Webserver.Http
{
    public class BeforeHandleRequestResult
    {
        public HttpServerResponse Response { get; }

        public BeforeHandleRequestResult(HttpServerResponse response)
        {
            Response = response;
        }
    }
}