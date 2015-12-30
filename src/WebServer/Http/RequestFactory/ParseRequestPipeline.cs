using Devkoes.Restup.WebServer.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Devkoes.Restup.WebServer.Http.RequestFactory
{
    internal class ParseRequestPipeline
    {
        private const uint BUFFER_SIZE = 8192;

        private IEnumerable<IRequestPipelinePart> GetPipeline()
        {
            return new IRequestPipelinePart[]
            {
                    new MethodParser(),
                    new ResourceIdentifierParser(),
                    new ProtocolVersionParser(),
                    new HeaderParser(),
                    new ContentParser()
            };
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

                            streamData = ConcatArrays(streamData, newStreamdata.Data);
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

        private T[] ConcatArrays<T>(T[] array1, T[] array2)
        {
            int array1OriginalLength = array1.Length;
            Array.Resize<T>(ref array1, array1OriginalLength + array2.Length);
            Array.Copy(array2, 0, array1, array1OriginalLength, array2.Length);

            return array1;
        }
    }
}
