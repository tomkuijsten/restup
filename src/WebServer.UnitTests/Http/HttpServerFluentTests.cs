using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Restup.HttpMessage;
using Restup.HttpMessage.Headers.Response;
using Restup.Webserver.Http;
using Restup.Webserver.UnitTests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restup.Webserver.UnitTests.Http
{
    public class HttpServerFluentTests
    {
        private readonly HttpServer _httpServer;
        private readonly List<HttpServerResponse> _responses;

        private readonly Dictionary<string, EchoRouteHandler> _routeHandlers;

        public HttpServerFluentTests Given => this;
        public HttpServerFluentTests When => this;
        public HttpServerFluentTests Then => this;

        public HttpServerFluentTests()
        {
            _httpServer = new HttpServer(80);
            _responses = new List<HttpServerResponse>();
            _routeHandlers = new Dictionary<string, EchoRouteHandler>();
        }

        public HttpServerFluentTests ListeningOnDefaultRoute()
        {
            var routeHandler = new EchoRouteHandler();
            _routeHandlers[string.Empty] = routeHandler;
            _httpServer.RegisterRoute(routeHandler);
            return this;
        }

        public HttpServerFluentTests ListeningOnRoute(string urlPrefix)
        {
            var routeHandler = new EchoRouteHandler();
            _routeHandlers[urlPrefix] = routeHandler;
            _httpServer.RegisterRoute(urlPrefix, routeHandler);
            return this;
        }

        public HttpServerFluentTests RequestHasArrived(string uri, IEnumerable<string> acceptEncodings = null, byte[] content = null)
        {
            var httpServerRequest = Utils.CreateHttpRequest(uri: new Uri(uri, UriKind.Relative), acceptEncodings: acceptEncodings, content: content);
            var response = _httpServer.HandleRequestAsync(httpServerRequest).Result;
            _responses.Add(response);
            return this;
        }

        public HttpServerFluentTests AssertLastResponse<T>(Func<HttpServerResponse, T> actualFunc, T expected)
        {
            var response = _responses.Last();
            var actual = actualFunc(response);

            Assert.AreEqual(expected, actual);
            return this;
        }

        public HttpServerFluentTests AssertLastResponse<T>(Func<ContentEncodingHeader, T> actualFunc, T expected)
        {
            var response = _responses.Last();
            var header = response.Headers.OfType<ContentEncodingHeader>().First();
            var actual = actualFunc(header);

            Assert.AreEqual(expected, actual);
            return this;
        }

        public HttpServerFluentTests AssertRouteHandlerRequest<T>(Func<IHttpServerRequest, T> actualFunc, T expected)
        {
            var routeHandler = _routeHandlers.Single().Value;
            var request = routeHandler.Requests.Last();
            var actual = actualFunc(request);

            Assert.AreEqual(expected, actual);
            return this;
        }

        public HttpServerFluentTests AssertRouteHandlerReceivedRequest()
        {
            var routeHandler = _routeHandlers.Single().Value;
            Assert.AreEqual(1, routeHandler.Requests.Count());
            return this;
        }

        public HttpServerFluentTests AssertRouteHandlerReceivedNoRequests()
        {
            var routeHandler = _routeHandlers.Single().Value;
            Assert.AreEqual(0, routeHandler.Requests.Count());
            return this;
        }

        public HttpServerFluentTests AssertRouteHandlerRequest<T>(string prefix, Func<IHttpServerRequest, T> actualFunc, T expected)
        {
            var routeHandler = _routeHandlers[prefix];
            var request = routeHandler.Requests.Last();
            var actual = actualFunc(request);

            Assert.AreEqual(expected, actual);
            return this;
        }

        public HttpServerFluentTests AssertRouteHandlerReceivedRequest(string prefix)
        {
            var routeHandler = _routeHandlers[prefix];
            Assert.AreEqual(1, routeHandler.Requests.Count());
            return this;
        }

        public HttpServerFluentTests AssertRouteHandlerReceivedNoRequests(string prefix)
        {
            var routeHandler = _routeHandlers[prefix];
            Assert.AreEqual(0, routeHandler.Requests.Count());
            return this;
        }
    }
}