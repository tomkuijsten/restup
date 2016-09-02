using Restup.HttpMessage;
using Restup.HttpMessage.Headers.Response;
using Restup.HttpMessage.Models.Contracts;
using Restup.HttpMessage.Models.Schemas;
using Restup.Webserver.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Restup.WebServer.Logging;

namespace Restup.Webserver.Http
{
    public class HttpServer : IDisposable
    {
        private readonly int _port;
        private readonly StreamSocketListener _listener;
        private readonly SortedSet<RouteRegistration> _routes;
        private readonly ContentEncoderFactory _contentEncoderFactory;
        private ILogger _log;

        public HttpServer(int serverPort)
        {
            _log = LogManager.GetLogger<HttpServer>();
            _port = serverPort;
            _routes = new SortedSet<RouteRegistration>();
            _listener = new StreamSocketListener();

            _listener.ConnectionReceived += ProcessRequestAsync;
            _contentEncoderFactory = new ContentEncoderFactory();
        }

        public async Task StartServerAsync()
        {
            await _listener.BindServiceNameAsync(_port.ToString());

            _log.Info($"Webserver listening on port {_port}");
        }

        public void StopServer()
        {
            ((IDisposable)this).Dispose();

            _log.Info($"Webserver stopped listening on port {_port}");
        }

        /// <summary>
        /// Registers the <see cref="IRouteHandler"/> on the root url.
        /// </summary>
        /// <param name="restRoutehandler">The rest route handler to register.</param>
        public void RegisterRoute(IRouteHandler restRoutehandler)
        {
            RegisterRoute("/", restRoutehandler);
        }

        /// <summary>
        /// Registers the <see cref="IRouteHandler"/> on the specified url prefix.
        /// </summary>
        /// <param name="urlPrefix">The urlprefix to use, e.g. /api, /api/v001, etc. </param>
        /// <param name="restRoutehandler">The rest route handler to register.</param>
        public void RegisterRoute(string urlPrefix, IRouteHandler restRoutehandler)
        {
            var routeRegistration = new RouteRegistration(urlPrefix, restRoutehandler);

            if (_routes.Contains(routeRegistration))
            {
                throw new Exception($"RouteHandler already registered for prefix: {urlPrefix}");
            }

            _routes.Add(routeRegistration);
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
                        var httpResponse = await HandleRequestAsync(request);

                        await WriteResponseAsync(httpResponse, args.Socket);
                    }
                }
                catch (Exception ex)
                {
                    _log.Error($"Exception while handling process: {ex.Message}");
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

        internal async Task<HttpServerResponse> HandleRequestAsync(IHttpServerRequest request)
        {
            var routeRegistration = _routes.FirstOrDefault(x => x.Match(request));
            if (routeRegistration == null)
            {
                return HttpServerResponse.Create(new Version(1, 1), HttpResponseStatus.BadRequest);
            }

            HttpServerResponse httpResponse;
            if (request.Method == HttpMethod.OPTIONS)
            {
                // max age possible by chrome https://code.google.com/p/chromium/codesearch#chromium/src/third_party/WebKit/Source/core/loader/CrossOriginPreflightResultCache.cpp&l=40&rcl=1399481969
                httpResponse = HttpServerResponse.Create(HttpResponseStatus.OK);
                httpResponse.AddHeader(new AccessControlAllowMethodsHeader(new[] { HttpMethod.GET, HttpMethod.POST, HttpMethod.PUT, HttpMethod.DELETE,HttpMethod.OPTIONS,  }));
                httpResponse.AddHeader(new AccessControlMaxAgeHeader(10 * 60));
                if (request.AccessControlRequestHeaders.Any())
                    httpResponse.AddHeader(new AccessControlAllowHeadersHeader(request.AccessControlRequestHeaders));
            }
            else
            {
                httpResponse = await routeRegistration.HandleAsync(request);
            }

            var responseWithContentEncoding = await AddContentEncodingAsync(httpResponse, request.AcceptEncodings);
            return AddCorsHeaders(request, responseWithContentEncoding);
        }

        private static HttpServerResponse AddCorsHeaders(IHttpServerRequest request, HttpServerResponse responseWithContentEncoding)
        {
            if (!string.IsNullOrWhiteSpace(request.Origin))
                responseWithContentEncoding.AddHeader(new AccessControlAllowOriginHeader("*"));

            return responseWithContentEncoding;
        }

        private async Task<HttpServerResponse> AddContentEncodingAsync(HttpServerResponse httpResponse, IEnumerable<string> acceptEncodings)
        {
            var contentEncoder = _contentEncoderFactory.GetEncoder(acceptEncodings);
            var encodedContent = await contentEncoder.Encode(httpResponse.Content);

            var newResponse = HttpServerResponse.Create(httpResponse.HttpVersion, httpResponse.ResponseStatus);

            foreach (var header in httpResponse.Headers)
            {
                newResponse.AddHeader(header);
            }
            newResponse.Content = encodedContent;
            newResponse.AddHeader(new ContentLengthHeader(encodedContent?.Length ?? 0));

            var contentEncodingHeader = contentEncoder.ContentEncodingHeader;
            AddHeaderIfNotNull(contentEncodingHeader, newResponse);

            return newResponse;
        }

        private static void AddHeaderIfNotNull(IHttpHeader contentEncodingHeader, HttpServerResponse newResponse)
        {
            if (contentEncodingHeader != null)
            {
                newResponse.AddHeader(contentEncodingHeader);
            }
        }

        private static async Task WriteResponseAsync(HttpServerResponse response, StreamSocket socket)
        {
            using (var output = socket.OutputStream)
            {
                await output.WriteAsync(response.ToBytes().AsBuffer());
                await output.FlushAsync();
            }
        }

        void IDisposable.Dispose()
        {
            _listener.Dispose();
        }
    }
}
