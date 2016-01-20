using Devkoes.HttpMessage.Models.Schemas;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Devkoes.HttpMessage.UnitTests
{
    public class HttpServerResponseTestBytes
    {
        private void AssertDefaultFirstLineOnly(string responseMessage)
        {
            Assert.AreEqual("HTTP/1.1 200 OK\r\n\r\n", responseMessage);
        }

        [TestMethod]
        public void Create_EmptyResponse_HttpFirstLineOnly()
        {
            var response = HttpServerResponse.Create(HttpResponseStatus.OK);

            var responseMessage = response.ToString();

            AssertDefaultFirstLineOnly(responseMessage);
        }
    }
}
