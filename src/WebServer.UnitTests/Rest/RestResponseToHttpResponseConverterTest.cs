using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Rest;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Devkoes.Restup.WebServer.UnitTests.Visitors
{
    [TestClass]
    public class RestResponseVisitorTest
    {
        [TestMethod]
        public void Visit_Delete_DefaultResponse()
        {
            RestToHttpResponseConverter v = new RestToHttpResponseConverter();
            var httpResponse = v.ConvertToHttpResponse(new DeleteResponse(DeleteResponse.ResponseStatus.OK), default(RestServerRequest));

            string content = httpResponse.ToString();

            StringAssert.Contains(content, "200 OK");
            StringAssert.Contains(content, "Connection: ");
            StringAssert.Contains(content, "Date: ");
        }
    }
}
