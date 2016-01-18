using Devkoes.HttpMessage.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace Devkoes.HttpMessage.RequestParsers
{
    public class HttpRequestParser
    {
        private const uint BUFFER_SIZE = 8192;

        private IEnumerable<IHttpRequestPartParser> GetPipeline()
        {
            return new IHttpRequestPartParser[]
            {
                    new MethodParser(),
                    new ResourceIdentifierParser(),
                    new ProtocolVersionParser(),
                    new HeaderParser(),
                    new ContentParser()
            };
        }

        public async Task<HttpRequest> ParseRequestStream(StreamSocket httpRequestSocket)
        {
            using (var inputStream = httpRequestSocket.InputStream)
            {
                return await ParseRequestStream(inputStream);
            }
        }

        public async Task<HttpRequest> ParseRequestStream(IInputStream requestStream)
        {
            var httpStream = new HttpRequestStream(requestStream);
            var request = new HttpRequest();

            try
            {
                var stream = await httpStream.ReadAsync(BUFFER_SIZE, InputStreamOptions.Partial);
                byte[] streamData = stream.Data;

                var requestPipeline = GetPipeline();
                using (var pipeLineEnumerator = requestPipeline.GetEnumerator())
                {
                    pipeLineEnumerator.MoveNext();
                    bool requestComplete = false;

                    while (!requestComplete)
                    {
                        pipeLineEnumerator.Current.HandleRequestPart(streamData, request);
                        streamData = pipeLineEnumerator.Current.UnparsedData;

                        if (pipeLineEnumerator.Current.IsFinished)
                        {
                            if (!pipeLineEnumerator.Current.IsSucceeded ||
                                !pipeLineEnumerator.MoveNext())
                            {
                                break;
                            }
                        }
                        else
                        {
                            var newStreamdata = await httpStream.ReadAsync(BUFFER_SIZE, InputStreamOptions.Partial);

                            if (!newStreamdata.ReadSuccessful)
                            {
                                break;
                            }

                            streamData = streamData.ConcatArray(newStreamdata.Data);
                        }
                    }
                }

                request.IsComplete = requestPipeline.All(p => p.IsSucceeded);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return request;
        }
    }
}
