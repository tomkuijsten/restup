using Devkoes.Restup.WebServer.Builders;
using Devkoes.Restup.WebServer.Models.Schemas;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq;

namespace WebServer.UnitTests.Builders
{
    [TestClass]
    public class RestRequestBuilderTest
    {
        private string _get_valid_jsonContent =
@"GET /api/Parameters/2/nodes/12 HTTP/1.1
Host: localhost
Accept: text/xml
Content-Type: application/json

{Parameter:23.0}";

        [TestMethod]
        public void Build_WithJSONBody()
        {
            var reqBuilder = new RestRequestBuilder();
            var request = reqBuilder.Build(_get_valid_jsonContent);

            Assert.AreEqual(MediaType.JSON, request.BodyMediaType);
            Assert.AreEqual(MediaType.XML, request.AcceptHeaders.First());
            Assert.AreEqual("{Parameter:23.0}", request.Body);
            Assert.AreEqual("api/Parameters/2/nodes/12", request.Uri);
            Assert.AreEqual(RestVerb.GET, request.Verb);
        }
    }
}
