using Restup.HttpMessage.Models.Schemas;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using Restup.Webserver.Rest;
using Restup.Webserver.UnitTests.TestHelpers;
using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Restup.Webserver.UnitTests.Rest
{
    [TestClass]
    public class RestRouteHandlerTests_UriFormatValidation
    {
        private RestRouteHandler restHandler;

        [TestInitialize()]
        public void Initialize()
        {
            restHandler = new RestRouteHandler();
        }

        private class TwoUriFormatWithSameNameAndMethodController
        {
            [UriFormat("/Get")]
            public IGetResponse Get() => new GetResponse(GetResponse.ResponseStatus.OK);

            [UriFormat("/Get")]
            public IGetResponse Get2() => new GetResponse(GetResponse.ResponseStatus.OK);
        }

        [TestMethod]
        public async Task RegisterController_OneControllerWithTwoMethodsWithSameNameAndMethod_ThrowsException()
        {
            AssertRegisterControllerThrows<TwoUriFormatWithSameNameAndMethodController>();
            await AssertHandleRequest("/Get", HttpMethod.GET, HttpResponseStatus.BadRequest);
        }

        private class OnePostMethodController
        {
            [UriFormat("/Post")]
            public IPostResponse Post() => new PostResponse(PostResponse.ResponseStatus.Created);
        }

        [TestMethod]
        public async Task RegisterController_OneControllerRegisteredTwice_ThrowsException()
        {
            restHandler.RegisterController<OnePostMethodController>();
            await AssertHandleRequest("/Post", HttpMethod.POST, HttpResponseStatus.Created);
            AssertRegisterControllerThrows<OnePostMethodController>();
            await AssertHandleRequest("/Post", HttpMethod.POST, HttpResponseStatus.Created);
        }

        private class OnePostMethodWithParameterizedConstructorController
        {
            [UriFormat("/Post")]
            public IPostResponse Post() => new PostResponse(PostResponse.ResponseStatus.Created);

            public OnePostMethodWithParameterizedConstructorController(string param)
            {
            }
        }

        [TestMethod]
        public async Task RegisterController_OneParameterizedControllerRegisteredTwice_ThrowsException()
        {
            restHandler.RegisterController<OnePostMethodWithParameterizedConstructorController>("param");
            await AssertHandleRequest("/Post", HttpMethod.POST, HttpResponseStatus.Created);
            AssertRegisterControllerThrows<OnePostMethodWithParameterizedConstructorController>("param");
            await AssertHandleRequest("/Post", HttpMethod.POST, HttpResponseStatus.Created);
        }

        [TestMethod]
        public async Task RegisterController_TwoDifferentControllersWithSimilarlyNamedMethodsAndVerbs_ThrowsException()
        {
            restHandler.RegisterController<OnePostMethodController>();
            await AssertHandleRequest("/Post", HttpMethod.POST, HttpResponseStatus.Created);
            AssertRegisterControllerThrows<OnePostMethodWithParameterizedConstructorController>("param");
            await AssertHandleRequest("/Post", HttpMethod.POST, HttpResponseStatus.Created);
        }

        private void AssertRegisterControllerThrows<T>(params string[] args) where T : class
        {
            Assert.ThrowsException<Exception>(() =>
                restHandler.RegisterController<T>(args)
            );
        }

        private void AssertRegisterControllerThrows<T>() where T : class
        {
            Assert.ThrowsException<Exception>(() =>
                restHandler.RegisterController<T>()
            );
        }

        private async Task AssertHandleRequest(string uri, HttpMethod method, HttpResponseStatus expectedStatus)
        {
            var request = Utils.CreateHttpRequest(uri: new Uri(uri, UriKind.Relative), method: method);
            var result = await restHandler.HandleRequest(request);

            Assert.AreEqual(expectedStatus, result.ResponseStatus);
        }
    }
}
