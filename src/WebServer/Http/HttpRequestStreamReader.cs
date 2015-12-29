using System;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace Devkoes.Restup.WebServer.Http
{
    internal class HttpRequestStreamReader
    {
        private const uint BUFFER_SIZE = 8192;
        private const int INPUTSTREAM_EMPTY_RETRY_COUNT = 3;
        // This should be an exception, so 10ms is not that big a deal.
        private const int INPUTSTREAM_EMPTY_DELAYMS = 10;

        private Regex _findContentLengthRegex = new Regex("content-length:\\D*(?<cl>\\d*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        internal Encoding HttpRequestStringEncoding { get; }

        internal HttpRequestStreamReader()
        {
            HttpRequestStringEncoding = Encoding.GetEncoding("iso-8859-1");
        }

        internal async Task<string> GetRequestString(StreamSocket socket)
        {
            string currentRequestPart = null;
            using (IInputStream input = socket.InputStream)
            {
                currentRequestPart = await GetRequestString(input);
            }

            return TrimEndNullChars(currentRequestPart);
        }

        private class RequestStreamData
        {
            public string Request { get; set; }
            public int? ExpectedContentLength { get; set; }
            public int? BodyStartIndex { get; set; }
            public int NothingFoundCounter { get; set; }
            public bool ContinueReading { get; set; }
        }

        internal async Task<string> GetRequestString(IInputStream input)
        {
            var reqStreamData = new RequestStreamData() { ContinueReading = true };

            IBuffer buffer = (new byte[BUFFER_SIZE]).AsBuffer();

            while (reqStreamData.ContinueReading)
            {
                var readBytes = await input.ReadAsync(buffer, BUFFER_SIZE, InputStreamOptions.Partial);

                if (readBytes.Length == 0)
                {
                    await HandleEmptyRead(reqStreamData);
                    continue;
                }

                reqStreamData.Request += HttpRequestStringEncoding.GetString(readBytes.ToArray());

                if (!reqStreamData.BodyStartIndex.HasValue)
                {
                    FindBodyIndex(reqStreamData);
                }

                if (reqStreamData.BodyStartIndex.HasValue &&
                    !reqStreamData.ExpectedContentLength.HasValue)
                {
                    FindContentLength(reqStreamData);
                }

                CheckIfReadingIsDone(reqStreamData);
            }

            CheckLengthForErrors(reqStreamData);

            return reqStreamData.Request;
        }

        private void CheckIfReadingIsDone(RequestStreamData reqStreamData)
        {
            if (string.IsNullOrEmpty(reqStreamData.Request))
            {
                return;
            }

            if (!reqStreamData.ExpectedContentLength.HasValue)
            {
                return;
            }

            if (reqStreamData.ExpectedContentLength == 0 &&
                reqStreamData.BodyStartIndex.HasValue)
            {
                reqStreamData.ContinueReading = false;
            }

            var requestLength = reqStreamData.Request.Length;
            var maxAllowedLength = reqStreamData.BodyStartIndex + reqStreamData.ExpectedContentLength;

            if (requestLength >= maxAllowedLength)
            {
                reqStreamData.ContinueReading = false;
            }
        }

        private void CheckLengthForErrors(RequestStreamData reqStreamData)
        {
            if (string.IsNullOrEmpty(reqStreamData.Request))
            {
                return;
            }

            if (!reqStreamData.ExpectedContentLength.HasValue)
            {
                return;
            }

            var requestLength = reqStreamData.Request.Length;
            var maxAllowedLength = reqStreamData.BodyStartIndex + reqStreamData.ExpectedContentLength;

            if (requestLength > maxAllowedLength)
            {
                reqStreamData.Request = null;
            }
        }

        private void FindContentLength(RequestStreamData reqStreamData)
        {
            Match m = _findContentLengthRegex.Match(reqStreamData.Request);
            if (!m.Success)
            {
                reqStreamData.ExpectedContentLength = 0;
                return;
            }

            int contentLength;
            if (!int.TryParse(m.Groups["cl"].Value, out contentLength))
            {
                // Content-Length was formatted inappropriate, bad request!
                reqStreamData.ContinueReading = false;
                reqStreamData.Request = null;
            }
            else
            {
                reqStreamData.ExpectedContentLength = contentLength;
            }
        }

        private async Task HandleEmptyRead(RequestStreamData reqStreamData)
        {
            // Nothing is found, but appearantly the body content is not complete yet.
            // Give the stream some time to process incoming data.

            Debug.WriteLine("No data in request stream available, wait 10ms...");
            reqStreamData.NothingFoundCounter += 1;

            if (reqStreamData.NothingFoundCounter >= INPUTSTREAM_EMPTY_RETRY_COUNT)
            {
                reqStreamData.Request = null;
                reqStreamData.ContinueReading = false;
            }
            else
            {
                await Task.Delay(INPUTSTREAM_EMPTY_DELAYMS);
            }
        }

        private static bool HasExpectedContentLength(string currentRequestPart, int expectedContentLength, int bodyStartIndex)
        {
            return expectedContentLength != -1 &&
                            currentRequestPart != null &&
                            currentRequestPart.Length != bodyStartIndex + expectedContentLength;
        }

        private void FindBodyIndex(RequestStreamData reqStreamData)
        {
            int headerEndIndex = reqStreamData.Request.IndexOf("\r\n\r\n");

            if (headerEndIndex > -1)
            {
                reqStreamData.BodyStartIndex = headerEndIndex + 4;
            }
        }

        internal string TrimEndNullChars(string input)
        {
            return input.TrimEnd(Convert.ToChar(0));
        }
    }
}
