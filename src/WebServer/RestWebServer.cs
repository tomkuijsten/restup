using System;
using System.Threading.Tasks;
using Devkoes.HttpMessage;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Rest;

namespace Devkoes.Restup.WebServer
{
    [Obsolete("Use HttpServer and RegisterRoute instead")]
    public class RestWebServer : IDisposable
    {
        private readonly HttpServer _httpServer;
        private readonly RestRouteHandler _restRouteHandler;

        public RestWebServer(int port, string urlPrefix)
        {
            var httpServer = new HttpServer(port);
            _restRouteHandler = new RestRouteHandler();
            httpServer.RegisterRoute(urlPrefix, _restRouteHandler);

            _httpServer = httpServer;
        }

        public RestWebServer(int port) : this(port, null)
        {
            
        }

        public RestWebServer() : this(8800, null)
        {
            
        }

        public void RegisterController<T>() where T : class
        {
            _restRouteHandler.RegisterController<T>();
        }

        public void RegisterController<T>(params object[] args) where T : class
        {
            _restRouteHandler.RegisterController<T>(() => args);
        }

        public void RegisterController<T>(Func<object[]> args) where T : class
        {
            _restRouteHandler.RegisterController<T>(args);
        }

        internal Task<HttpServerResponse> HandleRequest(IHttpServerRequest request)
        {
            return _httpServer.HandleRequest(request);
        }

        public async Task StartServerAsync()
        {
            await _httpServer.StartServerAsync();
        }

        public void Dispose()
        {
            ((IDisposable) _httpServer).Dispose();
        }
    }
}
