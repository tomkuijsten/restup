using Devkoes.Restup.WebServer.Models.Contracts;
using System;
using System.Diagnostics;
using System.IO;
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

        private void ProcessRequestAsync(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            // Don't lock this thread which handles incoming requests, release it asap.
            Task.Run(async () =>
            {
                try
                {
                    string request = await GetRequestString(args.Socket);

                    var result = await HandleRequest(request);

                    await WriteResponseAsync(result, args.Socket);
                }
                catch (Exception)
                {
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
                    await input.ReadAsync(buffer, BUFFER_SIZE, InputStreamOptions.Partial);
                    request.Append(HttpRequestStringEncoding.GetString(data, 0, data.Length));
                    dataRead = buffer.Length;
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
            using (Stream resp = output.AsStreamForWrite())
            {
                await resp.WriteAsync(response.RawResponse, 0, response.RawResponse.Length);
                await resp.FlushAsync();
            }
        }

        public void Dispose()
        {
            _listener.Dispose();
        }
    }
}
