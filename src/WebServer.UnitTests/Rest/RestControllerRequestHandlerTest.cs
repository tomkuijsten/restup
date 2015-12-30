using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Rest;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Devkoes.Restup.WebServer.UnitTests.Rest
{
    [TestClass]
    public class RestControllerRequestHandlerTest
    {
        [TestMethod]
        public void GetRestMethods_HasAsyncMethod_IsAsyncSet()
        {
            var restHandler = new RestControllerRequestHandler(null);

            var allDefs = restHandler.GetRestMethods<AsyncTestController>();

            Assert.AreEqual(1, allDefs.Count());
            Assert.AreEqual(true, allDefs.First().IsAsync);
        }

        [RestController(InstanceCreationType.Singleton)]
        public class AsyncTestController
        {
            [UriFormat("/users/{userId}")]
            public async Task<GetResponse> GetUser(int userId)
            {
                return await Task.Run(() => new GetResponse(GetResponse.ResponseStatus.OK, "test"));
            }
        }
    }
}
