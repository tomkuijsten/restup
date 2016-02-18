using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Devkoes.HttpMessage;
using Devkoes.HttpMessage.Models.Schemas;

namespace Devkoes.Restup.WebServer.Http
{
    public class HttpServer : IDisposable
    {
        private readonly int _port;
        private readonly StreamSocketListener _listener;
        private readonly List<RouteRegistration> _routes;

        public HttpServer(int serverPort)
        {            
            _listener = new StreamSocketListener();
            _listener.ConnectionReceived += ProcessRequestAsync;

            _port = serverPort;            

            _routes = new List<RouteRegistration>();
        }
       
        public async Task StartServerAsync()
        {
            await _listener.BindServiceNameAsync(_port.ToString());

            Debug.WriteLine($"Webserver started on port {_port}");
        }

        public void StopServer()
        {
            ((IDisposable)this).Dispose();

            Debug.WriteLine($"Webserver on port {_port} stopped");
        }

        private async void ProcessRequestAsync(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            await Task.Run(async () =>
            {
                try
                {
                    using (var inputStream = args.Socket.InputStream)
                    {
                        var request = await MutableHttpServerRequest.Parse(inputStream);

                        var httpResponse = await HandleRequest(request);

                        await WriteResponseAsync(httpResponse, args.Socket);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Exception while handling process: {ex.Message}");
                }
                finally
                {
                    try
                    {
                        args.Socket.Dispose();
                    }
                    catch { }
                }
            });
        }

        internal Task<HttpServerResponse> HandleRequest(IHttpServerRequest request)
        {
            var routeRegistration = _routes.FirstOrDefault(x => x.Match(request));
            if (routeRegistration == null)
                return Task.FromResult(new HttpServerResponse(new Version(1, 1), HttpResponseStatus.BadRequest));

            return routeRegistration.Handle(request);
        }

        private async Task WriteResponseAsync(HttpServerResponse response, StreamSocket socket)
        {
            using (IOutputStream output = socket.OutputStream)
            {
                await output.WriteAsync(response.ToBytes().AsBuffer());
                await output.FlushAsync();
            }
        }

        void IDisposable.Dispose()
        {
            _listener.Dispose();
        }

        /// <summary>
        /// Registers the <see cref="IRouteHandler"/> on the root url.
        /// </summary>
        /// <param name="restRoutehandler">The rest route handler to register.</param>
        public void RegisterRoute(IRouteHandler restRoutehandler)
        {
            RegisterRoute(string.Empty, restRoutehandler);
        }

        /// <summary>
        /// Registers the <see cref="IRouteHandler"/> on the specified url prefix.
        /// </summary>
        /// <param name="urlPrefix">The urlprefix to use, e.g. /api, /api/v001, etc. </param>
        /// <param name="restRoutehandler">The rest route handler to register.</param>
        public void RegisterRoute(string urlPrefix, IRouteHandler restRoutehandler)
        {
            _routes.Add(new RouteRegistration(urlPrefix, restRoutehandler));
        }

        private class RouteRegistration
        {
            private readonly IRouteHandler routeHandler;
            private readonly string urlPrefix;

            public RouteRegistration(string urlPrefix, IRouteHandler routeHandler)
            {
                this.urlPrefix = urlPrefix.FormatRelativeUri();
                this.routeHandler = routeHandler;
            }

            public bool Match(IHttpServerRequest request)
            {
                return request.Uri.ToString().StartsWith(urlPrefix);
            }

            private static IHttpServerRequest CreateHttpRequestWithUnprefixedUrl(IHttpServerRequest request, string prefix)
            {
                return new HttpServerRequest(request.Headers, request.Method, RemovePrefix(request.Uri, prefix), request.HttpVersion,
                    request.ContentTypeCharset, request.AcceptCharsets, request.ContentLength, request.ContentType,
                    request.AcceptMediaTypes, request.Content, request.IsComplete);
            }

            private static Uri RemovePrefix(Uri uri, string prefix)
            {
                if (string.IsNullOrWhiteSpace(prefix))
                    return uri;

                var uriToString = uri.ToString();
                if (uriToString.StartsWith(prefix))
                    uriToString = uriToString.Remove(0, prefix.Length);

                return new Uri(uriToString, UriKind.Relative);
            }

            public Task<HttpServerResponse> Handle(IHttpServerRequest request)
            {
                var unPrefixedRequest = CreateHttpRequestWithUnprefixedUrl(request, urlPrefix);

                return routeHandler.HandleRequest(unPrefixedRequest);
            }
        }
    }
}
