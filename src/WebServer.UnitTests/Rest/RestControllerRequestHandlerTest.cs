using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Rest;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using WebServer.Rest.Models.Contracts;

namespace Devkoes.Restup.WebServer.UnitTests.Rest
{
    [TestClass]
    public class RestControllerRequestHandlerTest
    {
        [TestMethod]
        public void GetRestMethods_HasAsyncTaskMethod_IsAsyncSet()
        {
            var restHandler = new RestControllerRequestHandler();

            var allDefs = restHandler.GetRestMethods<AsyncTaskTestController>(() => null);

            Assert.AreEqual(1, allDefs.Count());
            Assert.AreEqual(true, allDefs.First().IsAsync);
        }

        [TestMethod]
        public void GetRestMethods_HasIAsyncOperationMethod_IsAsyncSet()
        {
            var restHandler = new RestControllerRequestHandler();

            var allDefs = restHandler.GetRestMethods<AsyncOperationTestController>(() => null);

            Assert.AreEqual(1, allDefs.Count());
            Assert.AreEqual(true, allDefs.First().IsAsync);
        }

        [RestController(InstanceCreationType.Singleton)]
        public class AsyncTaskTestController
        {
            [UriFormat("/users/{userId}")]
            public async Task<GetResponse> GetUser(int userId)
            {
                return await Task.Run(() => new GetResponse(GetResponse.ResponseStatus.OK, "test"));
            }
        }

        [RestController(InstanceCreationType.Singleton)]
        public class AsyncOperationTestController
        {
            [UriFormat("/users/{userId}")]
            public IAsyncOperation<IGetResponse> GetUser(int userId)
            {
                return Task.Run(() => (IGetResponse) new GetResponse(GetResponse.ResponseStatus.OK, "test")).AsAsyncOperation();
            }
        }
    }
}
