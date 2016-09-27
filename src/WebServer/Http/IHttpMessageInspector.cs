using Restup.HttpMessage;

namespace Restup.Webserver.Http
{
    internal interface IHttpMessageInspector
    {
        BeforeHandleRequestResult BeforeHandleRequest(IHttpServerRequest request);
        AfterHandleRequestResult AfterHandleRequest(IHttpServerRequest request, HttpServerResponse httpResponse);
    }
}