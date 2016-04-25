using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Restup.Webserver.Models.Schemas;
using Restup.Webserver.Rest;

namespace Restup.Webserver.UnitTests.Visitors
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
