using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using Restup.Webserver.Rest;
using Restup.Webserver.UnitTests.TestHelpers;
using System;

namespace Restup.Webserver.UnitTests.Rest
{
    [TestClass]
    public class RestControllerRequestHandlerWithMultipleSimilarlyNamedMethods
    {
        [TestMethod]
        [DataRow("/Get", "/Get")]
        [DataRow("/Get/tom", "/Get/{name}")]
        [DataRow("/Get/henk/age/15", "/Get/{name}/age/{age}")]
        public void GetRestMethods_WithControllerThatHasMethodsSortedByNumberOfParametersAscending_CanHandleRequest(string uri, string route)
        {
            var restHandler = new RestControllerRequestHandler();
            restHandler.RegisterController<MethodsSortedByParametersAscendingController>();

            var request = restHandler.HandleRequest(Utils.CreateRestServerRequest(uri: new Uri(uri, UriKind.Relative)));

            Assert.IsNotNull(request.Result);
            Assert.AreEqual((int)GetResponse.ResponseStatus.OK, request.Result.StatusCode);
            Assert.AreEqual(route, (request.Result as IGetResponse)?.ContentData);
        }

        [TestMethod]
        [DataRow("/Get", "/Get")]
        [DataRow("/Get/tom", "/Get/{name}")]
        [DataRow("/Get/henk/age/15", "/Get/{name}/age/{age}")]
        public void GetRestMethods_WithControllerThatHasMethodsSortedByNumberOfParametersDescending_CanHandleRequest(string uri, string route)
        {
            var restHandler = new RestControllerRequestHandler();
            restHandler.RegisterController<MethodsSortedByParametersDescendingController>();

            var request = restHandler.HandleRequest(Utils.CreateRestServerRequest(uri: new Uri(uri, UriKind.Relative)));

            Assert.IsNotNull(request.Result);
            Assert.AreEqual((int)GetResponse.ResponseStatus.OK, request.Result.StatusCode);
            Assert.AreEqual(route, (request.Result as IGetResponse)?.ContentData);
        }

        [RestController(InstanceCreationType.Singleton)]
        public class MethodsSortedByParametersAscendingController
        {
            [UriFormat("/Get")]
            public IGetResponse Get() { return new GetResponse(GetResponse.ResponseStatus.OK, "/Get"); }

            [UriFormat("/Get/{name}")]
            public IGetResponse Get(string name) { return new GetResponse(GetResponse.ResponseStatus.OK, "/Get/{name}"); }

            [UriFormat("/Get/{name}/age/{age}")]
            public IGetResponse Get(string name, string age) { return new GetResponse(GetResponse.ResponseStatus.OK, "/Get/{name}/age/{age}"); }
        }

        [RestController(InstanceCreationType.Singleton)]
        public class MethodsSortedByParametersDescendingController
        {
            [UriFormat("/Get/{name}/age/{age}")]
            public IGetResponse Get(string name, string age) { return new GetResponse(GetResponse.ResponseStatus.OK, "/Get/{name}/age/{age}"); }

            [UriFormat("/Get/{name}")]
            public IGetResponse Get(string name) { return new GetResponse(GetResponse.ResponseStatus.OK, "/Get/{name}"); }

            [UriFormat("/Get")]
            public IGetResponse Get() { return new GetResponse(GetResponse.ResponseStatus.OK, "/Get"); }
        }
    }
}