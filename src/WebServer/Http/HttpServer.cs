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

namespace Restup.Webserver.Http
{
    public class HttpServer : IDisposable
    {
        private readonly int _port;
        private readonly StreamSocketListener _listener;
        private readonly SortedSet<RouteRegistration> _routes;
        private readonly ContentEncoderFactory _contentEncoderFactory;

        public HttpServer(int serverPort)
        {
            _port = serverPort;
            _routes = new SortedSet<RouteRegistration>();
            _listener = new StreamSocketListener();

            _listener.ConnectionReceived += ProcessRequestAsync;
            _contentEncoderFactory = new ContentEncoderFactory();
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

        internal async Task<HttpServerResponse> HandleRequestAsync(IHttpServerRequest request)
        {
            var routeRegistration = _routes.FirstOrDefault(x => x.Match(request));
            if (routeRegistration == null)
            {
                return HttpServerResponse.Create(new Version(1, 1), HttpResponseStatus.BadRequest);
            }

            var httpResponse = await routeRegistration.HandleAsync(request);
            return await AddContentEncodingAsync(httpResponse, request.AcceptEncodings);
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
