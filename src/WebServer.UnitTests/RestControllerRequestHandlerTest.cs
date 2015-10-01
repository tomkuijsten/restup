using Devkoes.Restup.WebServer;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.UnitTests
{
    [TestClass]
    public class RestControllerRequestHandlerTest
    {
        [TestMethod]
        public void Test()
        {
            var x = new RestControllerRequestHandler();

            var allDefs = x.GetValidMethodDefinitions<AsyncTestController>();

            Assert.AreEqual(1, allDefs.Count());
            //Assert.AreEqual(true, allDefs.First().IsAsync);
        }

        [RestController(InstanceCreationType.Singleton)]
        public class AsyncTestController
        {
            [UriFormat("/users/{userId}")]
            public async Task<GetResponse> GetUser(int userId)
            {
                return await Task.Run(() =>new GetResponse(GetResponse.ResponseStatus.OK, "test"));
            }
        }
    }
}
