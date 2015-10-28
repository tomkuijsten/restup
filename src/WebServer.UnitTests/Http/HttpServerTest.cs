using Devkoes.Restup.WebServer.Http;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devkoes.Restup.WebServer.Models.Contracts;

namespace WebServer.UnitTests.Http
{
    [TestClass]
    public class HttpServerTest
    {
        private class HttpTestServer : HttpServer
        {
            public HttpTestServer() : base(10){}

            internal override Task<IHttpResponse> HandleRequest(string request)
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void TrimEndNullChars_MultipleNullChars_Removed()
        {
            string baseText = "just a small test";
            var s = new HttpTestServer();
            var input = baseText + s.HttpRequestStringEncoding.GetString(new[] { (byte)0, (byte)0, (byte)0 });
            var result = s.TrimEndNullChars(input);

            Assert.AreEqual(baseText, result);
        }
    }
}
