using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Visitors;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace WebServer.UnitTests.Visitors
{
    [TestClass]
    public class RestResponseVisitorTest
    {
        [TestMethod]
        public void Visit_Delete_DefaultResponse()
        {
            RestResponseVisitor v = new RestResponseVisitor(null);
            var httpResponse = v.Visit(new DeleteResponse(DeleteResponse.ResponseStatus.OK));

            StringAssert.Contains(httpResponse.Response, "200 OK");
            StringAssert.Contains(httpResponse.Response, "Connection: ");
            StringAssert.Contains(httpResponse.Response, "Date: ");
        }

        //todo, add more
    }
}
