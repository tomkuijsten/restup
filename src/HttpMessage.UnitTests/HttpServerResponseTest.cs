using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Restup.HttpMessage.Models.Schemas;
using System;
using System.Text;

namespace Restup.HttpMessage.UnitTests
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
        public void Create_SetAndRemoveContent_FirstLineOnly()
        {
            var response = HttpServerResponse.Create(HttpResponseStatus.OK);
            response.Content = Encoding.UTF8.GetBytes("data");
            response.Content = null;

            var responseMessage = response.ToString();
            var responseBytes = response.ToBytes();

            AssertDefaultFirstLineOnly(responseMessage, responseBytes);
        }

        [TestMethod]
        public void Create_ContentType_ContentTypeHeader()
        {
            var response = HttpServerResponse.Create(HttpResponseStatus.OK);
            response.ContentType = "application/json";

            var responseMessage = response.ToString();
            var responseBytes = response.ToBytes();

            string expectedValue = "HTTP/1.1 200 OK\r\nContent-Type: application/json\r\n\r\n";
            AssertResponse(expectedValue, responseMessage, responseBytes);
        }

        [TestMethod]
        public void Create_SetAndRemoveContentType_FirstLineOnly()
        {
            var response = HttpServerResponse.Create(HttpResponseStatus.OK);
            response.ContentType = "application/json";
            response.ContentType = null;

            var responseMessage = response.ToString();
            var responseBytes = response.ToBytes();

            AssertDefaultFirstLineOnly(responseMessage, responseBytes);
        }

        [TestMethod]
        public void Create_ContentTypeAndCharset_ContentTypeHeaderWithCharset()
        {
            var response = HttpServerResponse.Create(HttpResponseStatus.OK);
            response.ContentType = "application/json";
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
            response.ContentType = "application/json";
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
