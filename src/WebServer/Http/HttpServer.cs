using Devkoes.Restup.WebServer.Models.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public abstract IRestResponse HandleRequest(string request);


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

                var result = HandleRequest(request.ToString());

                using (IOutputStream output = socket.OutputStream)
                {
                    await WriteResponseAsync(result, output);
                }
            }
            catch (Exception)
            {
            }
        }

        private async Task WriteResponseAsync(IRestResponse response, IOutputStream os)
        {
            using (Stream resp = os.AsStreamForWrite())
            {
                // Look in the Data subdirectory of the app package
                byte[] bodyArray = Encoding.UTF8.GetBytes(response.Data);
                MemoryStream stream = new MemoryStream(bodyArray);
                string header = string.Format("HTTP/1.1 {0} {1}\r\n" +
                                  "Content-Length: {2}\r\n" +
                                  "Content-Type: application/json\r\n" +
                                  "Connection: close\r\n\r\n",
                                  response.StatusCode,
                                  HttpHelpers.GetHttpStatusCodeText(response.StatusCode),
                                  stream.Length);
                byte[] headerArray = Encoding.UTF8.GetBytes(header);
                await resp.WriteAsync(headerArray, 0, headerArray.Length);
                await stream.CopyToAsync(resp);
                await resp.FlushAsync();
            }
        }
    }
}
