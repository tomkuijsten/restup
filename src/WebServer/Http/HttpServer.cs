using Devkoes.Restup.WebServer.Models.Contracts;
using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Devkoes.Restup.WebServer.Helpers;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.Diagnostics;

namespace Devkoes.Restup.WebServer.Http
{
    public abstract class HttpServer : IDisposable
    {
        private const uint BufferSize = 8192;
        private int port;
        private readonly StreamSocketListener listener;

        public HttpServer(int serverPort)
        {
            listener = new StreamSocketListener();
            port = serverPort;
            listener.ConnectionReceived += (s, e) => ProcessRequestAsync(e.Socket);
        }

        internal abstract IHttpResponse HandleRequest(string request);


        public async void StartServerAsync()
        {
            await listener.BindServiceNameAsync(port.ToString());

            Debug.WriteLine($"Webserver started on port {port}");
        }

        public void Dispose()
        {
            listener.Dispose();
        }

        private async void ProcessRequestAsync(StreamSocket socket)
        {
            try
            {
                // this works for text only
                string request = await GetRequestString(socket);

                var result = HandleRequest(request);

                await WriteResponseAsync(result, socket);
            }
            catch (Exception)
            {
            }
        }

        private static async Task<string> GetRequestString(StreamSocket socket)
        {
            StringBuilder request = new StringBuilder();
            using (IInputStream input = socket.InputStream)
            {
                byte[] data = new byte[BufferSize];
                IBuffer buffer = data.AsBuffer();
                uint dataRead = BufferSize;
                while (dataRead == BufferSize)
                {
                    await input.ReadAsync(buffer, BufferSize, InputStreamOptions.Partial);
                    request.Append(Encoding.UTF8.GetString(data, 0, data.Length));
                    dataRead = buffer.Length;
                }
            }

            return request.ToString();
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
    }
}
