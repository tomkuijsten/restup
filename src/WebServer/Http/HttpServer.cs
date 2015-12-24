using Devkoes.Restup.WebServer.Models.Contracts;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace Devkoes.Restup.WebServer.Http
{
    public abstract class HttpServer : IDisposable
    {
        private const uint BUFFER_SIZE = 8192;
        private readonly int _port;
        private readonly StreamSocketListener _listener;

        internal Encoding HttpRequestStringEncoding { get; }

        public HttpServer(int serverPort)
        {
            _listener = new StreamSocketListener();
            _port = serverPort;
            _listener.ConnectionReceived += ProcessRequestAsync;
            HttpRequestStringEncoding = Encoding.GetEncoding("iso-8859-1");
        }

        internal abstract Task<IHttpResponse> HandleRequest(string request);

        public async Task StartServerAsync()
        {
            await _listener.BindServiceNameAsync(_port.ToString());

            Debug.WriteLine($"Webserver started on port {_port}");
        }

        private async void ProcessRequestAsync(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            await Task.Run(async () =>
            {
                try
                {
                    string request = await GetRequestString(args.Socket);

                    var result = await HandleRequest(request);

                    await WriteResponseAsync(result, args.Socket);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Exception while handling process: {ex.Message}");
                }
                finally
                {
                    try
                    {
                        // Lets make sure the socket will always close
                        args.Socket.Dispose();
                    }
                    catch { }
                }
            });
        }

        private async Task<string> GetRequestString(StreamSocket socket)
        {
            StringBuilder request = new StringBuilder();
            using (IInputStream input = socket.InputStream)
            {
                byte[] data = new byte[BUFFER_SIZE];
                IBuffer buffer = data.AsBuffer();
                uint dataRead = BUFFER_SIZE;
                while (dataRead == BUFFER_SIZE)
                {
                    var readBytes = await input.ReadAsync(buffer, BUFFER_SIZE, InputStreamOptions.Partial);
                    request.Append(HttpRequestStringEncoding.GetString(readBytes.ToArray()));
                    dataRead = readBytes.Length;
                }
            }

            return TrimEndNullChars(request.ToString());
        }

        internal string TrimEndNullChars(string input)
        {
            return input.TrimEnd(Convert.ToChar(0));
        }

        private async Task WriteResponseAsync(IHttpResponse response, StreamSocket socket)
        {
            using (IOutputStream output = socket.OutputStream)
            {
                await output.WriteAsync(response.RawResponse.AsBuffer());
                await output.FlushAsync();
            }
        }

        public void Dispose()
        {
            _listener.Dispose();
        }
    }
}
