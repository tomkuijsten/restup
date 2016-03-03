using Devkoes.HttpMessage;
using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Models.Contracts;
using Devkoes.Restup.WebServer.UnitTests.TestHelpers;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Devkoes.Restup.WebServer.UnitTests
{
    [TestClass]
    public class HttpServerTests
    {
        [TestMethod]
        public void HandleRequest_RegisteredOnDefaultRoute_RoutesSuccesfully()
        {
            var httpServer = new HttpServer(80);
            var routeHandler = new TestRouteHandler();
            httpServer.RegisterRoute(routeHandler);

            var uri = new Uri("/Get", UriKind.Relative);
            var response = httpServer.HandleRequestAsync(Utils.CreateHttpRequest(uri: uri)).Result;

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpResponseStatus.OK, response.ResponseStatus);

            Assert.AreEqual(routeHandler.Requests.Count(), 1);
            Assert.AreEqual(routeHandler.Requests.First().Uri, uri);
        }

        [TestMethod]
        [DataRow("api")]
        [DataRow("/api")]
        [DataRow("api/")]
        public void HandleRequest_RegisteredOnPrefixedRoute_RoutesSuccesfully(string registeredPrefix)
        {
            var httpServer = new HttpServer(80);
            var routeHandler = new TestRouteHandler();
            httpServer.RegisterRoute(registeredPrefix, routeHandler);

            var uri = new Uri("/api/Get", UriKind.Relative);
            var response = httpServer.HandleRequestAsync(Utils.CreateHttpRequest(uri: uri)).Result;

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpResponseStatus.OK, response.ResponseStatus);

            Assert.AreEqual(routeHandler.Requests.Count(), 1);
            Assert.AreEqual(new Uri("/Get", UriKind.Relative), routeHandler.Requests.First().Uri);
        }

        [TestMethod]
        public void HandleRequest_OnNonRegisteredRoute_ReturnsBadRequest()
        {
            var httpServer = new HttpServer(80);

            var uri = new Uri("/api/Get", UriKind.Relative);
            var response = httpServer.HandleRequestAsync(Utils.CreateHttpRequest(uri: uri)).Result;

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpResponseStatus.BadRequest, response.ResponseStatus);
        }

        [TestMethod]
        public void GivenMultipleRouteHandlersAreAddedInSequentialOrder_WhenRequestIsReceivedOnApiRoute_ThenRequestIsSuccesfullyReceived()
        {
            var httpServer = new HttpServer(80);
            var anyRouteHandler = new TestRouteHandler();
            var apiRouteHandler = new TestRouteHandler();

            httpServer.RegisterRoute(anyRouteHandler);
            httpServer.RegisterRoute("api", apiRouteHandler);

            var apiUri = new Uri("/api/Get", UriKind.Relative);
            var response = httpServer.HandleRequestAsync(Utils.CreateHttpRequest(uri: apiUri)).Result;

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpResponseStatus.OK, response.ResponseStatus);

            Assert.AreEqual(apiRouteHandler.Requests.Count(), 1);
            Assert.AreEqual(anyRouteHandler.Requests.Count(), 0);
        }

        [TestMethod]
        public void GivenMultipleRouteHandlersAreAddedInSequentialOrder_WhenRequestIsReceivedOnAnyRoute_ThenRequestIsSuccesfullyReceived()
        {
            var httpServer = new HttpServer(80);
            var anyRouteHandler = new TestRouteHandler();
            var apiRouteHandler = new TestRouteHandler();

            httpServer.RegisterRoute(anyRouteHandler);
            httpServer.RegisterRoute("api", apiRouteHandler);

            var apiUri = new Uri("/index.html", UriKind.Relative);
            var response = httpServer.HandleRequestAsync(Utils.CreateHttpRequest(uri: apiUri)).Result;

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpResponseStatus.OK, response.ResponseStatus);

            Assert.AreEqual(apiRouteHandler.Requests.Count(), 0);
            Assert.AreEqual(anyRouteHandler.Requests.Count(), 1);
        }

        [TestMethod]
        public void GivenMultipleRouteHandlersAreAddedInReverseOrder_WhenRequestIsReceivedOnApiRoute_ThenRequestIsSuccesfullyReceived()
        {
            var httpServer = new HttpServer(80);
            var anyRouteHandler = new TestRouteHandler();
            var apiRouteHandler = new TestRouteHandler();

            httpServer.RegisterRoute("api", apiRouteHandler);
            httpServer.RegisterRoute(anyRouteHandler);

            var apiUri = new Uri("/api/Get", UriKind.Relative);
            var response = httpServer.HandleRequestAsync(Utils.CreateHttpRequest(uri: apiUri)).Result;

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpResponseStatus.OK, response.ResponseStatus);

            Assert.AreEqual(apiRouteHandler.Requests.Count(), 1);
            Assert.AreEqual(anyRouteHandler.Requests.Count(), 0);
        }

        [TestMethod]
        public void GivenMultipleRouteHandlersAreAddedInReverseOrder_WhenRequestIsReceivedOnAnyRoute_ThenRequestIsSuccesfullyReceived()
        {
            var httpServer = new HttpServer(80);
            var anyRouteHandler = new TestRouteHandler();
            var apiRouteHandler = new TestRouteHandler();

            httpServer.RegisterRoute("api", apiRouteHandler);
            httpServer.RegisterRoute(anyRouteHandler);

            var apiUri = new Uri("/index.html", UriKind.Relative);
            var response = httpServer.HandleRequestAsync(Utils.CreateHttpRequest(uri: apiUri)).Result;

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpResponseStatus.OK, response.ResponseStatus);

            Assert.AreEqual(apiRouteHandler.Requests.Count(), 0);
            Assert.AreEqual(anyRouteHandler.Requests.Count(), 1);
        }

        [TestMethod]
        public void GivenMultipleRouteHandlersAreBeingAddedWithTheSamePrefix_ThenAnExceptionShouldBeThrown()
        {
            var httpServer = new HttpServer(80);
            httpServer.RegisterRoute("api", new TestRouteHandler());

            Assert.ThrowsException<Exception>(() => httpServer.RegisterRoute("api", new TestRouteHandler()));
        }

        [TestMethod]
        public void GivenMultipleRouteHandlersAreBeingAddedOnTheCatchAllRoute_ThenAnExceptionShouldBeThrown()
        {
            var httpServer = new HttpServer(80);
            httpServer.RegisterRoute(new TestRouteHandler());

            Assert.ThrowsException<Exception>(() => httpServer.RegisterRoute(new TestRouteHandler()));
        }

        private class TestRouteHandler : IRouteHandler
        {
            private readonly List<IHttpServerRequest> _requests = new List<IHttpServerRequest>();

            internal IEnumerable<IHttpServerRequest> Requests => _requests;

            public Task<HttpServerResponse> HandleRequest(IHttpServerRequest request)
            {
                _requests.Add(request);
                return Task.FromResult(Utils.CreateOkHttpServerResponse());
            }
        }
    }
}
