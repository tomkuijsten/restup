using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Restup.HttpMessage.Models.Schemas;
using Restup.Webserver.Http;
using System;

namespace Restup.Webserver.UnitTests.Http
{
    [TestClass]
    public class HttpServerTests
    {
        [TestMethod]
        public void HandleRequest_RegisteredOnDefaultRoute_RoutesSuccesfully()
        {
            new HttpServerFluentTests()
                .Given
                    .ListeningOnDefaultRoute()
                .When
                    .RequestHasArrived("/Get")
                .Then
                    .AssertRouteHandlerReceivedRequest()
                    .AssertLastResponse(x => x.ResponseStatus, HttpResponseStatus.OK);
        }

        [TestMethod]
        [DataRow("api")]
        [DataRow("/api")]
        [DataRow("api/")]
        public void HandleRequest_RegisteredOnPrefixedRoute_RoutesSuccesfully(string registeredPrefix)
        {
            new HttpServerFluentTests()
               .Given
                   .ListeningOnRoute(registeredPrefix)
               .When
                   .RequestHasArrived("/api/Get")
               .Then
                   .AssertRouteHandlerReceivedRequest()
                   .AssertRouteHandlerRequest(x => x.Uri, new Uri("/Get", UriKind.Relative))
                   .AssertLastResponse(x => x.ResponseStatus, HttpResponseStatus.OK);
        }

        [TestMethod]
        public void HandleRequest_OnNonRegisteredRoute_ReturnsBadRequest()
        {
            new HttpServerFluentTests()
               .When
                   .RequestHasArrived("/api/Get")
               .Then
                   .AssertLastResponse(x => x.ResponseStatus, HttpResponseStatus.BadRequest);
        }

        [TestMethod]
        public void GivenMultipleRouteHandlersAreAddedInSequentialOrder_WhenRequestIsReceivedOnApiRoute_ThenRequestIsSuccesfullyReceived()
        {
            new HttpServerFluentTests()
               .Given
                   .ListeningOnDefaultRoute()
                   .ListeningOnRoute("/api")
               .When
                   .RequestHasArrived("/api/Get")
               .Then
                   .AssertRouteHandlerReceivedNoRequests(string.Empty)
                   .AssertRouteHandlerReceivedRequest("/api")
                   .AssertRouteHandlerRequest("/api", x => x.Uri, new Uri("/Get", UriKind.Relative))
                   .AssertLastResponse(x => x.ResponseStatus, HttpResponseStatus.OK);
        }

        [TestMethod]
        public void GivenMultipleRouteHandlersAreAddedInSequentialOrder_WhenRequestIsReceivedOnAnyRoute_ThenRequestIsSuccesfullyReceived()
        {
            new HttpServerFluentTests()
               .Given
                   .ListeningOnDefaultRoute()
                   .ListeningOnRoute("/api")
               .When
                   .RequestHasArrived("/index.html")
               .Then
                   .AssertRouteHandlerReceivedNoRequests("/api")
                   .AssertRouteHandlerReceivedRequest(string.Empty)
                   .AssertRouteHandlerRequest(string.Empty, x => x.Uri, new Uri("/index.html", UriKind.Relative))
                   .AssertLastResponse(x => x.ResponseStatus, HttpResponseStatus.OK);
        }

        [TestMethod]
        public void GivenMultipleRouteHandlersAreAddedInReverseOrder_WhenRequestIsReceivedOnApiRoute_ThenRequestIsSuccesfullyReceived()
        {
            new HttpServerFluentTests()
               .Given
                   .ListeningOnRoute("/api")
                   .ListeningOnDefaultRoute()
               .When
                   .RequestHasArrived("/api/Get")
               .Then
                   .AssertRouteHandlerReceivedNoRequests(string.Empty)
                   .AssertRouteHandlerReceivedRequest("/api")
                   .AssertRouteHandlerRequest("/api", x => x.Uri, new Uri("/Get", UriKind.Relative))
                   .AssertLastResponse(x => x.ResponseStatus, HttpResponseStatus.OK);
        }

        [TestMethod]
        public void GivenMultipleRouteHandlersAreAddedInReverseOrder_WhenRequestIsReceivedOnAnyRoute_ThenRequestIsSuccesfullyReceived()
        {
            new HttpServerFluentTests()
               .Given
                   .ListeningOnRoute("/api")
                   .ListeningOnDefaultRoute()
               .When
                   .RequestHasArrived("/index.html")
               .Then
                   .AssertRouteHandlerReceivedNoRequests("/api")
                   .AssertRouteHandlerReceivedRequest(string.Empty)
                   .AssertRouteHandlerRequest(string.Empty, x => x.Uri, new Uri("/index.html", UriKind.Relative))
                   .AssertLastResponse(x => x.ResponseStatus, HttpResponseStatus.OK);
        }

        [TestMethod]
        public void GivenMultipleRouteHandlersAreBeingAddedWithTheSamePrefix_ThenAnExceptionShouldBeThrown()
        {
            var httpServer = new HttpServer(80);
            httpServer.RegisterRoute("api", new EchoRouteHandler());

            Assert.ThrowsException<Exception>(() => httpServer.RegisterRoute("api", new EchoRouteHandler()));
        }

        [TestMethod]
        public void GivenMultipleRouteHandlersAreBeingAddedOnTheCatchAllRoute_ThenAnExceptionShouldBeThrown()
        {
            var httpServer = new HttpServer(80);
            httpServer.RegisterRoute(new EchoRouteHandler());

            Assert.ThrowsException<Exception>(() => httpServer.RegisterRoute(new EchoRouteHandler()));
        }
    }
}
