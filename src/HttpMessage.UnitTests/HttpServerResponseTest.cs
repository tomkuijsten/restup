using Devkoes.HttpMessage.Models.Schemas;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Devkoes.HttpMessage.UnitTests
{
    [TestClass]
    public class HttpServerResponseTest
    {
        private static Encoding DefaultHttpEncoding = Encoding.GetEncoding("iso-8859-1");
        private static Encoding DefaultJSONEncoding = Encoding.UTF8;

        private static void AssertDefaultFirstLineOnly(string responseMessage, byte[] responseBytes)
        {
            AssertResponse("HTTP/1.1 200 OK\r\n\r\n", responseMessage, responseBytes);
        }

        private static void AssertResponse(string expected, string responseMessage, byte[] responseBytes)
        {
            Assert.AreEqual(expected, responseMessage);
            CollectionAssert.AreEqual(DefaultHttpEncoding.GetBytes(expected), responseBytes);
        }

        private static void AssertContentResponse(
            string expectedStart,
            string expectedBody,
            HttpServerResponse response,
            string responseMessage,
            byte[] responseBytes)
        {
            Assert.AreEqual(expectedStart + expectedBody, responseMessage);
            List<byte> bytes = new List<byte>();
            bytes.AddRange(DefaultHttpEncoding.GetBytes(expectedStart));
            bytes.AddRange(response.ContentTypeEncoding.GetBytes(expectedStart));

            CollectionAssert.AreEqual(bytes, responseBytes);
        }

        [TestMethod]
        public void Create_EmptyResponse_HttpFirstLineOnly()
        {
            var response = HttpServerResponse.Create(HttpResponseStatus.OK);

            var responseMessage = response.ToString();
            var responseBytes = response.ToBytes();

            AssertDefaultFirstLineOnly(responseMessage, responseBytes);
        }

        [TestMethod]
        public void Create_HttpVersion2_HttpFirstLineOnly()
        {
            var response = HttpServerResponse.Create(new Version(2, 0), HttpResponseStatus.OK);

            var responseMessage = response.ToString();
            var responseBytes = response.ToBytes();

            string expectedValue = "HTTP/2.0 200 OK\r\n\r\n";
            AssertResponse(expectedValue, responseMessage, responseBytes);
        }

        [TestMethod]
        public void Create_LocationRedirect_LocationHeaderOnly()
        {
            var response = HttpServerResponse.Create(HttpResponseStatus.OK);
            response.Location = new Uri("/api/1", UriKind.Relative);

            var responseMessage = response.ToString();
            var responseBytes = response.ToBytes();

            string expectedValue = "HTTP/1.1 200 OK\r\nLocation: /api/1\r\n\r\n";
            AssertResponse(expectedValue, responseMessage, responseBytes);
        }

        [TestMethod]
        public void Create_SetAndremoveLocationRedirect_FirstLineOnly()
        {
            var response = HttpServerResponse.Create(HttpResponseStatus.OK);
            response.Location = new Uri("/api/1", UriKind.Relative);
            response.Location = null;

            var responseMessage = response.ToString();
            var responseBytes = response.ToBytes();

            AssertDefaultFirstLineOnly(responseMessage, responseBytes);
        }

        [TestMethod]
        public void Create_ContentWithUnknownCharacter_UnkownCharAsQuestionMark()
        {
            var response = HttpServerResponse.Create(HttpResponseStatus.OK);
            response.Content = "€";
            response.ContentType = MediaType.JSON;
            response.ContentCharset = "iso-8859-1"; // doesn't support €

            var responseBytes = response.ToBytes();

            var questionMarkByte = DefaultHttpEncoding.GetBytes("?");

            Assert.AreEqual(questionMarkByte.Single(), responseBytes.Last());
        }

        [TestMethod]
        public void Create_ContentWithUTF8Content_EncodedContent()
        {
            var response = HttpServerResponse.Create(HttpResponseStatus.OK);
            var expectedBody = "dat€";
            var expectedContentLength = 6; // the € sign takes 3 bytes in UTF-8
            response.Content = expectedBody;
            response.ContentType = MediaType.JSON;

            // UTF-8 is the default charset for json, should work without explicitly setting
            //response.ContentCharset = "utf-8";

            var responseMessage = response.ToString();
            var responseBytes = response.ToBytes();

            var expectedStart = $"HTTP/1.1 200 OK\r\nContent-Type: application/json\r\nContent-Length: {expectedContentLength}\r\n\r\n";


            List<byte> bytes = new List<byte>();
            bytes.AddRange(DefaultHttpEncoding.GetBytes(expectedStart));
            bytes.AddRange(DefaultJSONEncoding.GetBytes(expectedBody));

            Assert.AreEqual(expectedStart + expectedBody, responseMessage);
            CollectionAssert.AreEqual(bytes, responseBytes);
        }

        [TestMethod]
        public void Create_XmlContentWithoutExplicitCharset_EncodedContent()
        {
            var response = HttpServerResponse.Create(HttpResponseStatus.OK);
            var expectedBody = "dat€";
            response.Content = expectedBody;
            response.ContentType = MediaType.XML;

            // iso-8859-1 is the default charset for xml, which will not recognize the € char
            //response.ContentCharset = "utf-8";

            var responseBytes = response.ToBytes();
            var questionMarkByte = DefaultHttpEncoding.GetBytes("?");

            Assert.AreEqual(questionMarkByte.Single(), responseBytes.Last());
        }

        [TestMethod]
        public void Create_SetAndRemoveContent_FirstLineOnly()
        {
            var response = HttpServerResponse.Create(HttpResponseStatus.OK);
            response.Content = "data";
            response.Content = null;

            var responseMessage = response.ToString();
            var responseBytes = response.ToBytes();

            AssertDefaultFirstLineOnly(responseMessage, responseBytes);
        }

        [TestMethod]
        public void Create_ContentType_ContentTypeHeader()
        {
            var response = HttpServerResponse.Create(HttpResponseStatus.OK);
            response.ContentType = MediaType.JSON;

            var responseMessage = response.ToString();
            var responseBytes = response.ToBytes();

            string expectedValue = "HTTP/1.1 200 OK\r\nContent-Type: application/json\r\n\r\n";
            AssertResponse(expectedValue, responseMessage, responseBytes);
        }

        [TestMethod]
        public void Create_SetAndRemoveContentType_FirstLineOnly()
        {
            var response = HttpServerResponse.Create(HttpResponseStatus.OK);
            response.ContentType = MediaType.JSON;
            response.ContentType = null;

            var responseMessage = response.ToString();
            var responseBytes = response.ToBytes();

            AssertDefaultFirstLineOnly(responseMessage, responseBytes);
        }

        [TestMethod]
        public void Create_ContentTypeAndCharset_ContentTypeHeaderWithCharset()
        {
            var response = HttpServerResponse.Create(HttpResponseStatus.OK);
            response.ContentType = MediaType.JSON;
            response.ContentCharset = "utf-8";

            var responseMessage = response.ToString();
            var responseBytes = response.ToBytes();

            string expectedValue = "HTTP/1.1 200 OK\r\nContent-Type: application/json;charset=utf-8\r\n\r\n";
            AssertResponse(expectedValue, responseMessage, responseBytes);
        }

        [TestMethod]
        public void Create_UpdateContentCharset_UpdatedCharset()
        {
            var response = HttpServerResponse.Create(HttpResponseStatus.OK);
            response.ContentType = MediaType.JSON;
            response.ContentCharset = "utf-8";
            response.ContentCharset = "unicode";

            var responseMessage = response.ToString();
            var responseBytes = response.ToBytes();

            string expectedValue = "HTTP/1.1 200 OK\r\nContent-Type: application/json;charset=unicode\r\n\r\n";
            AssertResponse(expectedValue, responseMessage, responseBytes);
        }

        [TestMethod]
        public void Create_ConnectionClosed_ConnectionClosedHeader()
        {
            var response = HttpServerResponse.Create(HttpResponseStatus.OK);
            response.IsConnectionClosed = true;

            var responseMessage = response.ToString();
            var responseBytes = response.ToBytes();

            string expectedValue = "HTTP/1.1 200 OK\r\nConnection: close\r\n\r\n";
            AssertResponse(expectedValue, responseMessage, responseBytes);
        }

        [TestMethod]
        public void Create_SetAndUnsetConnectionClosed_FirstLineOnly()
        {
            var response = HttpServerResponse.Create(HttpResponseStatus.OK);
            response.IsConnectionClosed = true;
            response.IsConnectionClosed = false;

            var responseMessage = response.ToString();
            var responseBytes = response.ToBytes();

            AssertDefaultFirstLineOnly(responseMessage, responseBytes);
        }

        [TestMethod]
        public void Create_Date_DateInCorrectFormat()
        {
            var response = HttpServerResponse.Create(HttpResponseStatus.OK);
            response.Date = DateTime.Parse("Tue, 15 Nov 1994 08:12:31");

            var responseMessage = response.ToString();
            var responseBytes = response.ToBytes();

            string expectedValue = "HTTP/1.1 200 OK\r\nDate: Tue, 15 Nov 1994 08:12:31 GMT\r\n\r\n";
            AssertResponse(expectedValue, responseMessage, responseBytes);
        }

        [TestMethod]
        public void Create_SetAndRemoveDate_FirstLineOnly()
        {
            var response = HttpServerResponse.Create(HttpResponseStatus.OK);
            response.Date = DateTime.Parse("Tue, 15 Nov 1994 08:12:31");
            response.Date = null;

            var responseMessage = response.ToString();
            var responseBytes = response.ToBytes();

            AssertDefaultFirstLineOnly(responseMessage, responseBytes);
        }

        [TestMethod]
        public void Create_CustomHeader_AsHeader()
        {
            var response = HttpServerResponse.Create(HttpResponseStatus.OK);
            response.AddHeader("my-own-header", "23");

            var responseMessage = response.ToString();
            var responseBytes = response.ToBytes();

            string expectedValue = "HTTP/1.1 200 OK\r\nmy-own-header: 23\r\n\r\n";
            AssertResponse(expectedValue, responseMessage, responseBytes);
        }

        [TestMethod]
        public void Create_AddCustomHeaderTwice_LastSetHeader()
        {
            var response = HttpServerResponse.Create(HttpResponseStatus.OK);
            response.AddHeader("my-own-header", "23");
            response.AddHeader("my-own-header", "11");

            var responseMessage = response.ToString();
            var responseBytes = response.ToBytes();

            string expectedValue = "HTTP/1.1 200 OK\r\nmy-own-header: 11\r\n\r\n";
            AssertResponse(expectedValue, responseMessage, responseBytes);
        }

        [TestMethod]
        public void Create_AddAndRemoveCustomHeader_FirstLineOnly()
        {
            var response = HttpServerResponse.Create(HttpResponseStatus.OK);
            response.AddHeader("my-own-header", "23");
            response.RemoveHeader("my-own-header");

            var responseMessage = response.ToString();
            var responseBytes = response.ToBytes();

            AssertDefaultFirstLineOnly(responseMessage, responseBytes);
        }

        // TODO allow header
    }
}
