using Devkoes.HttpMessage;
using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.Restup.WebServer.UnitTests.TestHelpers;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Devkoes.Restup.HttpMessage.UnitTests
{
    [TestClass]
    public class HttpServerRequestTest
    {
        [TestMethod]
        public void ParseRequestStream_AllDataAtOnce_CompleteRequest()
        {
            var streamedRequest = "GET /api/data HTTP/1.1\r\nContent-Length: 4\r\n\r\ndata";
            var byteStreamParts = new List<byte[]>();
            byteStreamParts.Add(Encoding.UTF8.GetBytes(streamedRequest));

            var request = HttpServerRequest.Parse(new TestStream(byteStreamParts)).Result;

            Assert.AreEqual(HttpMethod.GET, request.Method);
            Assert.AreEqual(new Uri("/api/data", UriKind.Relative), request.Uri);
            Assert.AreEqual("data", request.Content);
            Assert.AreEqual(4, request.ContentLength);
            Assert.AreEqual("HTTP/1.1", request.HttpVersion);
            Assert.AreEqual(true, request.IsComplete);
        }

        [TestMethod]
        public void ParseRequestStream_AllHeaderTypes_AllHeadersParsed()
        {
            var streamedRequest = new[] {
                "GET /api/data HTTP/1.1",
                "Content-Length: 4",
                "Accept: application/json,text/xml",
                "Accept-Charset: utf-7;q=0.2, utf-8;q=0.1,*;q=0",
                "Content-Type: text/xml;charset=utf-8",
                "UnknownHeader: some:value",
                "",
                "data"
            };

            var byteStreamParts = new List<byte[]>();
            byteStreamParts.Add(Encoding.UTF8.GetBytes(string.Join("\r\n", streamedRequest)));

            var request = HttpServerRequest.Parse(new TestStream(byteStreamParts)).Result;

            Assert.AreEqual(true, request.IsComplete);
            Assert.IsTrue(request.Headers.Any(h => h.Name == "UnknownHeader" && h.Value == "some:value"));
            Assert.AreEqual(4, request.ContentLength);
            Assert.AreEqual("utf-8", request.ContentTypeCharset);
            Assert.AreEqual(MediaType.XML, request.ContentType);
            Assert.AreEqual("utf-7", request.AcceptCharsets.First());
            Assert.AreEqual(MediaType.JSON, request.AcceptMediaTypes.First());
        }

        [TestMethod]
        public void ParseRequestStream_AcceptWithQuality_QualityHighestAsAccept()
        {
            var streamedRequest = "GET /api/data HTTP/1.1\r\nAccept: application/json;q=0.5,text/xml;q=0.7\r\n\r\n";

            var byteStreamParts = Encoding.UTF8.GetBytes(streamedRequest);

            var request = HttpServerRequest.Parse(new TestStream(new[] { byteStreamParts })).Result;

            Assert.AreEqual(true, request.IsComplete);
            Assert.AreEqual(MediaType.XML, request.AcceptMediaTypes.First());
        }

        [TestMethod]
        public void ParseRequestStream_AcceptWithQuality_DefaultQuality1AsAccept()
        {
            var streamedRequest = "GET /api/data HTTP/1.1\r\nAccept: application/json,text/xml;q=0.7\r\n\r\n";

            var byteStreamParts = Encoding.UTF8.GetBytes(streamedRequest);

            var request = HttpServerRequest.Parse(new TestStream(new[] { byteStreamParts })).Result;

            Assert.AreEqual(true, request.IsComplete);
            Assert.AreEqual(MediaType.JSON, request.AcceptMediaTypes.First());
        }

        [TestMethod]
        public void ParseRequestStream_AcceptCharsetWithQuality_QualityHighestAsAccept()
        {
            var streamedRequest = "GET /api/data HTTP/1.1\r\nAccept-Charset: iso-8859-1;q=0.5,utf-8;q=0.7\r\n\r\n";

            var byteStreamParts = Encoding.UTF8.GetBytes(streamedRequest);

            var request = HttpServerRequest.Parse(new TestStream(new[] { byteStreamParts })).Result;

            Assert.AreEqual(true, request.IsComplete);
            Assert.AreEqual("utf-8", request.AcceptCharsets.First());
        }

        [TestMethod]
        public void ParseRequestStream_AcceptCharsetWithQuality_DefaultQuality1AsAccept()
        {
            var streamedRequest = "GET /api/data HTTP/1.1\r\nAccept-Charset: iso-8859-1,utf-8;q=0.7\r\n\r\n";

            var byteStreamParts = Encoding.UTF8.GetBytes(streamedRequest);

            var request = HttpServerRequest.Parse(new TestStream(new[] { byteStreamParts })).Result;

            Assert.AreEqual(true, request.IsComplete);
            Assert.AreEqual("iso-8859-1", request.AcceptCharsets.First());
        }

        [TestMethod]
        public void ParseRequestStream_ContentLengthNumberMissing_RequestIncomplete()
        {
            var streamedRequest = new[] {
                "GET /api/data HTTP/1.1",
                "Content-Length: four",
                "",
                "data"
            };

            var byteStreamParts = new List<byte[]>();
            byteStreamParts.Add(Encoding.UTF8.GetBytes(string.Join("\r\n", streamedRequest)));

            var request = HttpServerRequest.Parse(new TestStream(byteStreamParts)).Result;

            Assert.AreEqual(false, request.IsComplete);
        }

        [TestMethod]
        public void ParseRequestStream_TooMuchData_RequestIncomplete()
        {
            var streamedRequest = new[] {
                "GET /api/data HTTP/1.1",
                "Content-Length: 4",
                "",
                "data"
            };

            var extraData = "plusanotherextrafewbytes";
            var byteStreamParts = new List<byte[]>();
            byteStreamParts.Add(Encoding.UTF8.GetBytes(string.Join("\r\n", streamedRequest) + extraData));

            var request = HttpServerRequest.Parse(new TestStream(byteStreamParts)).Result;

            Assert.AreEqual(false, request.IsComplete);
        }

        [TestMethod]
        public void ParseRequestStream_DataOverflowSecondStream_ValidRequest()
        {
            var streamedRequest = new[] {
                "GET /api/data HTTP/1.1",
                "Content-Length: 4",
                "",
                "data"
            };

            var extraData = "plusanotherextrafewbytes";
            var byteStreamParts = new List<byte[]>();
            byteStreamParts.Add(Encoding.UTF8.GetBytes(string.Join("\r\n", streamedRequest)));
            byteStreamParts.Add(Encoding.UTF8.GetBytes(extraData));

            var request = HttpServerRequest.Parse(new TestStream(byteStreamParts)).Result;

            Assert.AreEqual(true, request.IsComplete);
        }

        [TestMethod]
        public void ParseRequestStream_PartedData_ValidRequest()
        {
            var streamedRequest = new[] {
                "GET /api/data HTTP/1.1",
                "Content-Length: 4\r\n", //to force double /r/n on string.Join
                ""
            };

            var content = "data";
            var byteStreamParts = new List<byte[]>();
            byteStreamParts.Add(Encoding.UTF8.GetBytes(string.Join("\r\n", streamedRequest)));
            byteStreamParts.Add(Encoding.UTF8.GetBytes(content));

            var request = HttpServerRequest.Parse(new TestStream(byteStreamParts)).Result;

            Assert.AreEqual(true, request.IsComplete);
            Assert.AreEqual(content, request.Content);
        }

        [TestMethod]
        public void ParseRequestStream_PartedDataWithEmptyReponseInBetween_ValidRequest()
        {
            var streamedRequest = new[] {
                "GET /api/data HTTP/1.1",
                "Content-Length: 4\r\n", //to force double /r/n on string.Join
                ""
            };

            var content = "data";
            var byteStreamParts = new List<byte[]>();
            byteStreamParts.Add(Encoding.UTF8.GetBytes(string.Join("\r\n", streamedRequest)));
            byteStreamParts.Add(new byte[] { });
            byteStreamParts.Add(Encoding.UTF8.GetBytes(content));

            var request = HttpServerRequest.Parse(new TestStream(byteStreamParts)).Result;

            Assert.AreEqual(true, request.IsComplete);
            Assert.AreEqual(content, request.Content);
        }

        [TestMethod]
        public void ParseRequestStream_DataLengthInSecondPart_ValidRequest()
        {
            var httpHeadersPart1 = "GET /api/data HTTP/1.1\r\n";
            var httpHeadersPart2 = "Content-Length: 4\r\n\r\n";
            var content = "data";
            var byteStreamParts = new List<byte[]>();
            byteStreamParts.Add(Encoding.UTF8.GetBytes(httpHeadersPart1));
            byteStreamParts.Add(Encoding.UTF8.GetBytes(httpHeadersPart2));
            byteStreamParts.Add(new byte[] { });
            byteStreamParts.Add(Encoding.UTF8.GetBytes(content));

            var request = HttpServerRequest.Parse(new TestStream(byteStreamParts)).Result;

            Assert.AreEqual(true, request.IsComplete);
            Assert.AreEqual(content, request.Content);
        }

        [TestMethod]
        public void ParseRequestStream_FragmentedData_ValidRequest()
        {
            var byteStreamParts = new List<byte[]>();
            byteStreamParts.Add(Encoding.UTF8.GetBytes("GET /api/data HTTP/1.1\r\n"));
            byteStreamParts.Add(Encoding.UTF8.GetBytes("Content-Leng"));
            byteStreamParts.Add(Encoding.UTF8.GetBytes("th: 4\r\n"));
            byteStreamParts.Add(Encoding.UTF8.GetBytes("\r\nd"));
            byteStreamParts.Add(Encoding.UTF8.GetBytes("a"));
            byteStreamParts.Add(Encoding.UTF8.GetBytes("t"));
            byteStreamParts.Add(Encoding.UTF8.GetBytes("a"));

            var request = HttpServerRequest.Parse(new TestStream(byteStreamParts)).Result;

            Assert.AreEqual(true, request.IsComplete);
            Assert.AreEqual("data", request.Content);
        }

        [TestMethod]
        public void ParseRequestStream_WithoutDataAndHeaders_CompleteRequest()
        {
            var streamedRequest = new[] {
                "GET /api/data HTTP/1.1\r\n", //to force double /r/n on string.Join
                ""
            };

            var byteStreamParts = new List<byte[]>();
            byteStreamParts.Add(Encoding.UTF8.GetBytes(string.Join("\r\n", streamedRequest)));

            var request = HttpServerRequest.Parse(new TestStream(byteStreamParts)).Result;

            Assert.AreEqual(true, request.IsComplete);
        }

        [TestMethod]
        public void ParseRequestStream_ThreeEmptyResponses_EmptyRequestString()
        {
            var streamedRequest = new[] {
                "GET /api/data HTTP/1.1",
                "Content-Length: 4\r\n", //to force double /r/n on string.Join
                ""
            };

            var body = "data";
            var byteStreamParts = new List<byte[]>();
            byteStreamParts.Add(Encoding.UTF8.GetBytes(string.Join("\r\n", streamedRequest)));
            byteStreamParts.Add(new byte[] { });
            byteStreamParts.Add(new byte[] { });
            byteStreamParts.Add(new byte[] { });
            byteStreamParts.Add(new byte[] { });
            byteStreamParts.Add(Encoding.UTF8.GetBytes(body));

            var request = HttpServerRequest.Parse(new TestStream(byteStreamParts)).Result;

            Assert.AreEqual(false, request.IsComplete);
        }
    }
}
