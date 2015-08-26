using Devkoes.Restup.WebServer.Builders;
using Devkoes.Restup.WebServer.Models.Schemas;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace WebServer.UnitTests.Builders
{
    [TestClass]
    public class RestRequestBuilderTest
    {
        private string _get_valid_jsonContent =
@"GET http://localhost/api/Parameters/2/nodes/12
Accept: text/xml
Content-Type: application/json

{Parameter:23.0}";

        [TestMethod]
        public void Build()
        {
            var reqBuilder = new RestRequestBuilder();
            var request = reqBuilder.Build(_get_valid_jsonContent);

            Assert.AreEqual(MediaType.JSON, request.BodyMediaType);
            Assert.AreEqual("{Parameter:23.0}", request.Body);
            Assert.AreEqual("http://localhost/api/Parameters/2/nodes/12", request.Uri);
            Assert.AreEqual(RestVerb.GET, request.Verb);
        }
    }
}
