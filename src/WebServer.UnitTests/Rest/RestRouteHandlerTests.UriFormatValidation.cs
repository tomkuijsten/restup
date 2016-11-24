using System;
using System.Threading.Tasks;
using Restup.HttpMessage.Models.Schemas;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using Restup.Webserver.UnitTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Restup.Webserver.UnitTests.Rest
{
    [TestClass]
    public class RestRouteHandlerTests_UriFormatValidation : RestRouteHandlerTests
    {
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
            _restRouteHandler.RegisterController<OnePostMethodController>();
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
            _restRouteHandler.RegisterController<OnePostMethodWithParameterizedConstructorController>("param");
            await AssertHandleRequest("/Post", HttpMethod.POST, HttpResponseStatus.Created);
            AssertRegisterControllerThrows<OnePostMethodWithParameterizedConstructorController>("param");
            await AssertHandleRequest("/Post", HttpMethod.POST, HttpResponseStatus.Created);
        }

        [TestMethod]
        public async Task RegisterController_TwoDifferentControllersWithSimilarlyNamedMethodsAndVerbs_ThrowsException()
        {
            _restRouteHandler.RegisterController<OnePostMethodController>();
            await AssertHandleRequest("/Post", HttpMethod.POST, HttpResponseStatus.Created);
            AssertRegisterControllerThrows<OnePostMethodWithParameterizedConstructorController>("param");
            await AssertHandleRequest("/Post", HttpMethod.POST, HttpResponseStatus.Created);
        }

        private class UriFormatWitMisnamedPathInUrlController
        {
            [UriFormat("/Get/{param}/")]
            public IGetResponse Get(string param2) => new GetResponse(GetResponse.ResponseStatus.OK, param2);
        }

        [TestMethod]
        public void RegisterController_AControllerWithMismatchedParametersInPath_ThrowsException()
        {
            AssertRegisterControllerThrows<UriFormatWitMisnamedPathInUrlController>();
        }

        private class UriFormatWitMisnamedUriParameterInUrlController
        {
            [UriFormat("/Get/?param={param}")]
            public IGetResponse Get(string param2) => new GetResponse(GetResponse.ResponseStatus.OK, param2);
        }

        [TestMethod]
        public void RegisterController_AControllerWithMismatchedParametersInUriParameters_ThrowsException()
        {
            AssertRegisterControllerThrows<UriFormatWitMisnamedUriParameterInUrlController>();
        }

        private class UriFormatWithMoreInUrlPathController
        {
            [UriFormat("/Get/{param}/{param2}")]
            public IGetResponse Get(string param) => new GetResponse(GetResponse.ResponseStatus.OK, param);
        }

        [TestMethod]
        public void RegisterController_AControllerWithMoreInUrlPath_ThrowsException()
        {
            AssertRegisterControllerThrows<UriFormatWithMoreInUrlPathController>();
        }

        private class UriFormatWithMoreInUrlParameterController
        {
            [UriFormat("/Get/?param={param}&param2={param}")]
            public IGetResponse Get(string param) => new GetResponse(GetResponse.ResponseStatus.OK, param);
        }

        [TestMethod]
        public void RegisterController_AControllerWithMoreInUrlParameter_ThrowsException()
        {
            AssertRegisterControllerThrows<UriFormatWithMoreInUrlParameterController>();
        }

        private async Task AssertHandleRequest(string uri, HttpMethod method, HttpResponseStatus expectedStatus)
        {
            var request = Utils.CreateHttpRequest(uri: new Uri(uri, UriKind.Relative), method: method);
            var result = await _restRouteHandler.HandleRequest(request);

            Assert.AreEqual(expectedStatus, result.ResponseStatus);
        }
    }
}
