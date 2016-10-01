using Restup.HttpMessage.Models.Contracts;
using Restup.HttpMessage.Plumbing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Restup.HttpMessage.ServerRequestParsers
{
    internal class HttpRequestParser
    {
        private const uint BUFFER_SIZE = 8192;

        internal static HttpRequestParser Default { get; }

        static HttpRequestParser()
        {
            Default = new HttpRequestParser();
        }

        private IEnumerable<IHttpRequestPartParser> GetPipeline()
        {
            return new IHttpRequestPartParser[]
            {
                    new MethodParser(),
                    new ResourceIdentifierParser(),
                    new ProtocolVersionParser(),
                    new HeadersParser(),
                    new ContentParser()
            };
        }

        internal async Task<MutableHttpServerRequest> ParseRequestStream(IInputStream requestStream)
        {
            var httpStream = new HttpRequestStream(requestStream);
            var request = new MutableHttpServerRequest();

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
