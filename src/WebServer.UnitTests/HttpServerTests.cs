using System;
using System.Threading.Tasks;
using Devkoes.HttpMessage;
using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.UnitTests.TestHelpers;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

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
            var response = httpServer.HandleRequest(Utils.CreateHttpRequest(uri: uri)).Result;
            
            Assert.IsNotNull(response);
            Assert.AreEqual(HttpResponseStatus.OK, response.ResponseStatus);

            Assert.IsNotNull(routeHandler.LastRequest);
            Assert.AreEqual(routeHandler.LastRequest.Uri, uri);
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
            var response = httpServer.HandleRequest(Utils.CreateHttpRequest(uri: uri)).Result;

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpResponseStatus.OK, response.ResponseStatus);

            Assert.IsNotNull(routeHandler.LastRequest);
            Assert.AreEqual(new Uri("/Get", UriKind.Relative), routeHandler.LastRequest.Uri);
        }

        [TestMethod]
        public void HandleRequest_OnNonRegisteredRoute_ReturnsBadRequest()
        {
            var httpServer = new HttpServer(80);

            var uri = new Uri("/api/Get", UriKind.Relative);
            var response = httpServer.HandleRequest(Utils.CreateHttpRequest(uri: uri)).Result;

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpResponseStatus.BadRequest, response.ResponseStatus);
        }

        private class TestRouteHandler : IRouteHandler
        {
            internal IHttpServerRequest LastRequest { get; private set; }

            public Task<HttpServerResponse> HandleRequest(IHttpServerRequest request)
            {
                LastRequest = request;
                return Task.FromResult(Utils.CreateOkHttpServerResponse());
            }            
        }
    }
}
