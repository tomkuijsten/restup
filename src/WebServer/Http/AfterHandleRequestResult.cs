using Restup.HttpMessage;

namespace Restup.Webserver.Http
{
    public class AfterHandleRequestResult
    {
        public HttpServerResponse Response { get; }

        public AfterHandleRequestResult(HttpServerResponse response)
        {
            Response = response;
        }
    }
}