using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Restup.Webserver.Models.Schemas;
using Restup.Webserver.Rest;

namespace Restup.Webserver.UnitTests.Visitors
{
    [TestClass]
    public class RestToHttpResponseConverterTests
    {
        [TestMethod]
        public void HeadersAreApplied()
        {
            var responseConverter = new RestToHttpResponseConverter();
            var restResponse = new GetResponse(GetResponse.ResponseStatus.OK, new Dictionary<string, string> { { "X-CustomHeader", "CustomValue" } }, null);
            var httpResponse = responseConverter.ConvertToHttpResponse(restResponse, default(RestServerRequest));

            Assert.AreEqual(httpResponse.Headers.First(x => x.Name == "X-CustomHeader").Value, "CustomValue");
        }
    }
}