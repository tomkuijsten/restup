using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Restup.HttpMessage;
using Restup.HttpMessage.Headers.Response;
using Restup.Webserver.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restup.Webserver.UnitTests.TestHelpers
{
    public class HttpServerTests_Fluent
    {
        private readonly HttpServer _httpServer;
        private readonly List<HttpServerResponse> _responses;

        private readonly Dictionary<string, EchoRouteHandler> _routeHandlers;

        public HttpServerTests_Fluent Given => this;
        public HttpServerTests_Fluent When => this;
        public HttpServerTests_Fluent Then => this;

        public HttpServerTests_Fluent()
        {
            _httpServer = new HttpServer(80);
            _responses = new List<HttpServerResponse>();
            _routeHandlers = new Dictionary<string, EchoRouteHandler>();
        }

        public HttpServerTests_Fluent ListeningOnDefaultRoute()
        {
            var routeHandler = new EchoRouteHandler();
            _routeHandlers[string.Empty] = routeHandler;
            _httpServer.RegisterRoute(routeHandler);
            return this;
        }

        public HttpServerTests_Fluent ListeningOnRoute(string urlPrefix)
        {
            var routeHandler = new EchoRouteHandler();
            _routeHandlers[urlPrefix] = routeHandler;
            _httpServer.RegisterRoute(urlPrefix, routeHandler);
            return this;
        }

        public HttpServerTests_Fluent RequestHasArrived(string uri, IEnumerable<string> acceptEncodings = null, byte[] content = null)
        {
            var httpServerRequest = Utils.CreateHttpRequest(uri: new Uri(uri, UriKind.Relative), acceptEncodings: acceptEncodings, content: content);
            var response = _httpServer.HandleRequestAsync(httpServerRequest).Result;
            _responses.Add(response);
            return this;
        }

        public HttpServerTests_Fluent AssertLastResponse<T>(Func<HttpServerResponse, T> actualFunc, T expected)
        {
            var response = _responses.Last();
            var actual = actualFunc(response);

            Assert.AreEqual(expected, actual);
            return this;
        }

        public HttpServerTests_Fluent AssertLastResponse<T>(Func<ContentEncodingHeader, T> actualFunc, T expected)
        {
            var response = _responses.Last();
            var header = response.Headers.OfType<ContentEncodingHeader>().First();
            var actual = actualFunc(header);

            Assert.AreEqual(expected, actual);
            return this;
        }

        public HttpServerTests_Fluent AssertRouteHandlerRequest<T>(Func<IHttpServerRequest, T> actualFunc, T expected)
        {
            var routeHandler = _routeHandlers.Single().Value;
            var request = routeHandler.Requests.Last();
            var actual = actualFunc(request);

            Assert.AreEqual(expected, actual);
            return this;
        }

        public HttpServerTests_Fluent AssertRouteHandlerReceivedRequest()
        {
            var routeHandler = _routeHandlers.Single().Value;
            Assert.AreEqual(1, routeHandler.Requests.Count());
            return this;
        }

        public HttpServerTests_Fluent AssertRouteHandlerReceivedNoRequests()
        {
            var routeHandler = _routeHandlers.Single().Value;
            Assert.AreEqual(0, routeHandler.Requests.Count());
            return this;
        }

        public HttpServerTests_Fluent AssertRouteHandlerRequest<T>(string prefix, Func<IHttpServerRequest, T> actualFunc, T expected)
        {
            var routeHandler = _routeHandlers[prefix];
            var request = routeHandler.Requests.Last();
            var actual = actualFunc(request);

            Assert.AreEqual(expected, actual);
            return this;
        }

        public HttpServerTests_Fluent AssertRouteHandlerReceivedRequest(string prefix)
        {
            var routeHandler = _routeHandlers[prefix];
            Assert.AreEqual(1, routeHandler.Requests.Count());
            return this;
        }

        public HttpServerTests_Fluent AssertRouteHandlerReceivedNoRequests(string prefix)
        {
            var routeHandler = _routeHandlers[prefix];
            Assert.AreEqual(0, routeHandler.Requests.Count());
            return this;
        }
    }
}