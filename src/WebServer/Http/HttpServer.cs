using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Restup.HttpMessage;
using Restup.HttpMessage.Headers.Response;
using Restup.HttpMessage.Models.Contracts;
using Restup.HttpMessage.Models.Schemas;
using Restup.WebServer.Logging;

namespace Restup.Webserver.Http
{
    public class HttpServer : IDisposable
    {
        private readonly int _port;
        private readonly StreamSocketListener _listener;
        private readonly SortedSet<RouteRegistration> _routes;
        private readonly ContentEncoderFactory _contentEncoderFactory;
        private readonly ILogger _log;
        private readonly List<IHttpMessageInspector> _messageInspectors;

        public HttpServer(HttpServerConfiguration configuration)
        {
            _log = LogManager.GetLogger<HttpServer>();
            _port = configuration.ServerPort;
            _listener = new StreamSocketListener();

            _listener.ConnectionReceived += ProcessRequestAsync;
            _contentEncoderFactory = new ContentEncoderFactory();
            _messageInspectors = new List<IHttpMessageInspector>();

            if (configuration.CorsConfiguration != null)
                _messageInspectors.Add(new CorsMessageInspector(configuration.CorsConfiguration.AllowedOrigins));

            _routes = new SortedSet<RouteRegistration>(configuration.Routes);
        }

        [Obsolete("Use constructor that takes a httpServerConfiguration")]
        public HttpServer(int serverPort)
            : this(new HttpServerConfiguration().ListenOnPort(serverPort))
        {
        }

        public async Task StartServerAsync()
        {
            _log.Info($"Binding webserver to port {_port}...");

            await _listener.BindServiceNameAsync(_port.ToString());

            _log.Info($"Webserver listening on port {_port}.");
        }

        public void StopServer()
        {
            ((IDisposable)this).Dispose();

            _log.Info($"Webserver stopped listening on port {_port}.");
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
                        if (!request.IsComplete)
                        {
                            _log.Warn($"Request {request.Method} {request.Uri} aborted.");
                            return;
                        }

                        if (_log.IsDebugEnabled())
                            _log.Debug($"Handling request {request.Method} {request.Uri} from {args.Socket.Information.LocalAddress}.");

                        var httpResponse = await HandleRequestAsync(request);

                        if (_log.IsDebugEnabled())
                            _log.Debug($"Request {request.Method} {request.Uri} from {args.Socket.Information.LocalAddress} handled, response status: {httpResponse.ResponseStatus}.");

                        await WriteResponseAsync(httpResponse, args.Socket);
                        if (_log.IsDebugEnabled())
                            _log.Debug($"Responded to request {request.Method} {request.Uri}, {args.Socket.Information.LocalAddress}.");
                    }
                }
                catch (Exception ex)
                {
                    _log.Error($"Exception while handling request from {args.Socket.Information.LocalAddress}", ex);
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

            var httpResponse = ApplyMessageInspectorsBeforeHandleRequest(request);

            if (httpResponse == null)
                httpResponse = await routeRegistration.HandleAsync(request);

            httpResponse = await AddContentEncodingAsync(httpResponse, request.AcceptEncodings);
            httpResponse = ApplyMessageInspectorsAfterHandleRequest(request, httpResponse);

            return httpResponse;
        }

        private HttpServerResponse ApplyMessageInspectorsBeforeHandleRequest(IHttpServerRequest request)
        {
            foreach (var httpMessageInspector in _messageInspectors)
            {
                var result = httpMessageInspector.BeforeHandleRequest(request);
                if (result != null)
                    return result.Response;
            }

            return null;
        }

        private HttpServerResponse ApplyMessageInspectorsAfterHandleRequest(IHttpServerRequest request,
            HttpServerResponse httpResponse)
        {
            foreach (var httpMessageInspector in _messageInspectors)
            {
                var result = httpMessageInspector.AfterHandleRequest(request, httpResponse);
                if (result != null)
                    httpResponse = result.Response;
            }
            return httpResponse;
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
            _log.Info("Disposing HttpServer...");
            _listener.Dispose();
            _log.Info("HttpServer disposed.");
        }
    }
}
