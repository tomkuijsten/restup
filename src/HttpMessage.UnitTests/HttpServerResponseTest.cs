using Devkoes.HttpMessage.Models.Schemas;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;

namespace Devkoes.HttpMessage.UnitTests
{
    [TestClass]
    public class HttpServerResponseTest
    {
        [TestMethod]
        public void Create_Valid_Valid()
        {
            var response = HttpServerResponse.Create(Version.Parse("1.1"), HttpResponseStatus.OK);

            var responseMessage = response.ToString();

            Assert.AreEqual(responseMessage, "HTTP/1.1 200 OK\r\n\r\n");
        }
    }
}
