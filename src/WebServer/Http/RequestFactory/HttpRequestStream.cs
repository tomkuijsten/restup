using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Devkoes.Restup.WebServer.Http.RequestFactory
{
    internal class HttpRequestStream
    {
        internal struct ReadResult
        {
            public byte[] Data { get; }
            public bool ReadSuccessful { get; }

            public ReadResult(byte[] data, bool successful)
            {
                Data = data;
                ReadSuccessful = successful;
            }
        }

        // This should be an exception, so 10ms is not that big a deal.
        private const int INPUTSTREAM_EMPTY_DELAYMS = 10;

        private const int INPUTSTREAM_EMPTY_RETRY_COUNT = 3;

        private IInputStream _requestStream;

        public HttpRequestStream(IInputStream requestStream)
        {
            _requestStream = requestStream;
        }

        internal async Task<ReadResult> ReadAsync(uint bufferSize, InputStreamOptions streamOptions)
        {
            IBuffer response = null;
            int retryCounter = 0;
            do
            {
                var buffer = (new byte[bufferSize]).AsBuffer();
                response = await _requestStream.ReadAsync(buffer, bufferSize, streamOptions);

                if (response.Length == 0)
                {
                    retryCounter++;
                    await Task.Delay(INPUTSTREAM_EMPTY_DELAYMS);
                }
            } while (response.Length == 0 && retryCounter < INPUTSTREAM_EMPTY_RETRY_COUNT);

            bool succeed = response.Length > 0;
            byte[] data = succeed ? response.ToArray() : new byte[0];

            return new ReadResult(data, succeed);
        }
    }
}
