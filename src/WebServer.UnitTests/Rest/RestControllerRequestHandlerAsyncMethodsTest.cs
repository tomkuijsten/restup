using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using Restup.Webserver.Rest;
using Restup.Webserver.UnitTests.TestHelpers;
using System;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Restup.Webserver.UnitTests.Rest
{
    [TestClass]
    public class RestControllerRequestHandlerAsyncMethodsTest
    {
        [TestMethod]
        public void GetRestMethods_HasTaskMethod_CanHandleRequest()
        {
            var restHandler = new RestControllerRequestHandler();
            restHandler.RegisterController<AsyncTaskTestController>();

            var request = restHandler.HandleRequest(Utils.CreateRestServerRequest(uri: new Uri("/users/1", UriKind.Relative)));

            Assert.IsNotNull(request.Result);
            Assert.AreEqual((int)GetResponse.ResponseStatus.OK, request.Result.StatusCode);
        }

        [TestMethod]
        public void GetRestMethods_HasIAsyncOperationMethod_CanHandleRequest()
        {
            var restHandler = new RestControllerRequestHandler();
            restHandler.RegisterController<AsyncOperationTestController>();

            var request = restHandler.HandleRequest(Utils.CreateRestServerRequest(uri: new Uri("/users/1", UriKind.Relative)));

            Assert.IsNotNull(request.Result);
            Assert.AreEqual((int)GetResponse.ResponseStatus.OK, request.Result.StatusCode);
        }

        [RestController(InstanceCreationType.Singleton)]
        public class AsyncTaskTestController
        {
            [UriFormat("/users/{userId}")]
            public async Task<GetResponse> GetUser(int userId)
            {
                return await Task.FromResult(new GetResponse(GetResponse.ResponseStatus.OK, "test"));
            }
        }

        [RestController(InstanceCreationType.Singleton)]
        public class AsyncOperationTestController
        {
            [UriFormat("/users/{userId}")]
            public IAsyncOperation<IGetResponse> GetUser(int userId)
            {
                return Task.FromResult<IGetResponse>(new GetResponse(GetResponse.ResponseStatus.OK, "test")).AsAsyncOperation();
            }
        }
    }
}
