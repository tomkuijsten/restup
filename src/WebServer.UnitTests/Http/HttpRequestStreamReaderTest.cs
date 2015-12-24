using Devkoes.Restup.WebServer.Http;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.Streams;

namespace WebServer.UnitTests.Http
{
    [TestClass]
    public class HttpRequestStreamReaderTest
    {
        private class TestStream : IInputStream
        {
            private int indexCounter = 0;
            private IEnumerable<byte[]> _byteStreamParts;

            public TestStream(IEnumerable<byte[]> byteStreamParts)
            {
                _byteStreamParts = byteStreamParts;
            }
            public void Dispose() { }

            public IAsyncOperationWithProgress<IBuffer, uint> ReadAsync(IBuffer buffer, uint count, InputStreamOptions options)
            {
                return AsyncInfo.Run<IBuffer, uint>((token, progress) =>
                {
                    IBuffer buff = new byte[] { }.AsBuffer();
                    if (indexCounter < _byteStreamParts.Count())
                    {
                        buff = _byteStreamParts.ElementAt(indexCounter).AsBuffer();
                    }

                    indexCounter++;
                    return Task.FromResult(buff);
                });
            }
        }

        [TestMethod]
        public void TrimEndNullChars_MultipleNullChars_Removed()
        {
            string baseText = "just a small test";
            var s = new HttpRequestStreamReader();
            var input = baseText + s.HttpRequestStringEncoding.GetString(new[] { (byte)0, (byte)0, (byte)0 });
            var result = s.TrimEndNullChars(input);

            Assert.AreEqual(baseText, result);
        }

        [TestMethod]
        public void GetRequestString_AllDataAtOnce_CompleteRequest()
        {
            var s = new HttpRequestStreamReader();

            var streamedRequest = "GET /api/data \r\nContent-Length: 4\r\n\r\ndata";
            var byteStreamParts = new List<byte[]>();
            byteStreamParts.Add(Encoding.UTF8.GetBytes(streamedRequest));

            var request = s.GetRequestString(new TestStream(byteStreamParts)).Result;

            Assert.AreEqual(streamedRequest, request);
        }

        [TestMethod]
        public void GetRequestString_ContentLengthNumberMissing_Null()
        {
            var s = new HttpRequestStreamReader();

            var streamedRequest = "GET /api/data \r\nContent-Length: four\r\n\r\ndata";
            var byteStreamParts = new List<byte[]>();
            byteStreamParts.Add(Encoding.UTF8.GetBytes(streamedRequest));

            var request = s.GetRequestString(new TestStream(byteStreamParts)).Result;

            Assert.IsNull(request);
        }

        [TestMethod]
        public void GetRequestString_ContentHeaderWrongCasing_CompleteRequest()
        {
            var s = new HttpRequestStreamReader();

            var streamedRequest = "GET /api/data \r\nconTenT-LenGth: 4\r\n\r\ndata";
            var byteStreamParts = new List<byte[]>();
            byteStreamParts.Add(Encoding.UTF8.GetBytes(streamedRequest));

            var request = s.GetRequestString(new TestStream(byteStreamParts)).Result;

            Assert.AreEqual(streamedRequest, request);
        }

        [TestMethod]
        public void GetRequestString_TooMuchData_DataTruncated()
        {
            var s = new HttpRequestStreamReader();

            var streamedRequest = "GET /api/data \r\nContent-Length: 4\r\n\r\ndata";
            var extraData = "plusanotherextrafewbytes";
            var byteStreamParts = new List<byte[]>();
            byteStreamParts.Add(Encoding.UTF8.GetBytes(streamedRequest + extraData));

            var request = s.GetRequestString(new TestStream(byteStreamParts)).Result;

            Assert.IsNull(request);
        }

        [TestMethod]
        public void GetRequestString_MissingData_DataTruncated()
        {
            var s = new HttpRequestStreamReader();

            var streamedRequest = "GET /api/data \r\nContent-Length: 3\r\n\r\ndata";
            var extraData = "plusanotherextrafewbytes";
            var byteStreamParts = new List<byte[]>();
            byteStreamParts.Add(Encoding.UTF8.GetBytes(streamedRequest + extraData));

            var request = s.GetRequestString(new TestStream(byteStreamParts)).Result;

            Assert.IsNull(request);
        }

        [TestMethod]
        public void GetRequestString_PartedData_CompleteRequest()
        {
            var s = new HttpRequestStreamReader();

            var httpHeadersPart1 = "GET /api/data \r\nContent-Length: 4\r\n\r\n";
            var body = "data";
            var byteStreamParts = new List<byte[]>();
            byteStreamParts.Add(Encoding.UTF8.GetBytes(httpHeadersPart1));
            byteStreamParts.Add(Encoding.UTF8.GetBytes(body));

            var request = s.GetRequestString(new TestStream(byteStreamParts)).Result;

            Assert.AreEqual(httpHeadersPart1 + body, request);
        }

        [TestMethod]
        public void GetRequestString_PartedDataWithEmptyReponseInBetween_CompleteRequest()
        {
            var s = new HttpRequestStreamReader();

            var httpHeadersPart1 = "GET /api/data \r\nContent-Length: 4\r\n\r\n";
            var body = "data";
            var byteStreamParts = new List<byte[]>();
            byteStreamParts.Add(Encoding.UTF8.GetBytes(httpHeadersPart1));
            byteStreamParts.Add(new byte[] { });
            byteStreamParts.Add(Encoding.UTF8.GetBytes(body));

            var request = s.GetRequestString(new TestStream(byteStreamParts)).Result;

            Assert.AreEqual(httpHeadersPart1 + body, request);
        }

        [TestMethod]
        public void GetRequestString_DataLengthInSecondPart_CompleteRequest()
        {
            var s = new HttpRequestStreamReader();

            var httpHeadersPart1 = "GET /api/data\r\n";
            var httpHeadersPart2 = "Content-Length: 4\r\n\r\n";
            var body = "data";
            var byteStreamParts = new List<byte[]>();
            byteStreamParts.Add(Encoding.UTF8.GetBytes(httpHeadersPart1));
            byteStreamParts.Add(Encoding.UTF8.GetBytes(httpHeadersPart2));
            byteStreamParts.Add(new byte[] { });
            byteStreamParts.Add(Encoding.UTF8.GetBytes(body));

            var request = s.GetRequestString(new TestStream(byteStreamParts)).Result;

            Assert.AreEqual(httpHeadersPart1 + httpHeadersPart2 + body, request);
        }

        [TestMethod]
        public void GetRequestString_FragmentedData_CompleteRequest()
        {
            var s = new HttpRequestStreamReader();

            var httpHeadersPart1 = "GET /api/data\r\n";
            var httpHeadersPart2 = "Content-Leng";
            var httpHeadersPart3 = "th: 4\r";
            var httpHeadersPart4 = "\n\r\n";
            var body1 = "d";
            var body2 = "a";
            var body3 = "d";
            var body4 = "a";

            var byteStreamParts = new List<byte[]>();
            byteStreamParts.Add(Encoding.UTF8.GetBytes(httpHeadersPart1));
            byteStreamParts.Add(Encoding.UTF8.GetBytes(httpHeadersPart2));
            byteStreamParts.Add(Encoding.UTF8.GetBytes(httpHeadersPart3));
            byteStreamParts.Add(Encoding.UTF8.GetBytes(httpHeadersPart4));
            byteStreamParts.Add(Encoding.UTF8.GetBytes(body1));
            byteStreamParts.Add(Encoding.UTF8.GetBytes(body2));
            byteStreamParts.Add(Encoding.UTF8.GetBytes(body3));
            byteStreamParts.Add(Encoding.UTF8.GetBytes(body4));

            var request = s.GetRequestString(new TestStream(byteStreamParts)).Result;

            string expectedString = new string(byteStreamParts.SelectMany(b => Encoding.UTF8.GetString(b)).ToArray());
            Assert.AreEqual(expectedString, request);
        }

        [TestMethod]
        public void GetRequestString_WithoutData_CompleteRequest()
        {
            var s = new HttpRequestStreamReader();

            var httpHeadersPart1 = "GET /api/data\r\n\r\n";
            var byteStreamParts = new List<byte[]>();
            byteStreamParts.Add(Encoding.UTF8.GetBytes(httpHeadersPart1));

            var request = s.GetRequestString(new TestStream(byteStreamParts)).Result;

            Assert.AreEqual(httpHeadersPart1, request);
        }

        [TestMethod]
        public void GetRequestString_ThreeEmptyResponses_EmptyRequestString()
        {
            var s = new HttpRequestStreamReader();

            var httpHeadersPart1 = "GET /api/data \r\nContent-Length: 4\r\n\r\n";
            var body = "data";
            var byteStreamParts = new List<byte[]>();
            byteStreamParts.Add(Encoding.UTF8.GetBytes(httpHeadersPart1));
            byteStreamParts.Add(new byte[] { });
            byteStreamParts.Add(new byte[] { });
            byteStreamParts.Add(new byte[] { });
            byteStreamParts.Add(new byte[] { });
            byteStreamParts.Add(Encoding.UTF8.GetBytes(body));

            var request = s.GetRequestString(new TestStream(byteStreamParts)).Result;

            Assert.IsNull(request);
        }
    }
}
