using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Restup.HttpMessage;
using Restup.HttpMessage.Models.Contracts;
using Restup.HttpMessage.Models.Schemas;
using Restup.Webserver.Http;

namespace Restup.Webserver.UnitTests.TestHelpers
{
    public class FluentHttpServerTests
    {
        private HttpServer _httpServer;
        private readonly List<HttpServerResponse> _responses;

        private readonly Dictionary<string, EchoRouteHandler> _routeHandlers;
        private readonly HttpServerConfiguration _configuration;

        public FluentHttpServerTests Given => this;
        public FluentHttpServerTests When => this;
        public FluentHttpServerTests Then => this;

        public FluentHttpServerTests()
        {
            _configuration = new HttpServerConfiguration();
            _httpServer = new HttpServer(_configuration);
            _responses = new List<HttpServerResponse>();
            _routeHandlers = new Dictionary<string, EchoRouteHandler>();
        }

        public FluentHttpServerTests ListeningOnDefaultRoute()
        {
            var routeHandler = new EchoRouteHandler();
            _routeHandlers[string.Empty] = routeHandler;
            _configuration.RegisterRoute(routeHandler);
            _httpServer = new HttpServer(_configuration);
            return this;
        }

        public FluentHttpServerTests ListeningOnRoute(string urlPrefix)
        {
            var routeHandler = new EchoRouteHandler();
            _routeHandlers[urlPrefix] = routeHandler;
            _configuration.RegisterRoute(urlPrefix, routeHandler);
            _httpServer = new HttpServer(_configuration);
            return this;
        }

        public FluentHttpServerTests RequestHasArrived(string uri, IEnumerable<string> acceptEncodings = null,
            byte[] content = null, string origin = null, HttpMethod? method = HttpMethod.GET, HttpMethod? accessControlRequestMethod = null,
            IEnumerable<string> accessControlRequestHeaders = null, string contentType = null)
        {
            var httpServerRequest = Utils.CreateHttpRequest(uri: new Uri(uri, UriKind.Relative),
                acceptEncodings: acceptEncodings, content: content, origin: origin, method: method, 
                accessControlRequestMethod: accessControlRequestMethod, accessControlRequestHeaders: accessControlRequestHeaders,
                contentType: contentType);
            var response = _httpServer.HandleRequestAsync(httpServerRequest).Result;
            _responses.Add(response);
            return this;
        }

        public FluentHttpServerTests AssertLastResponse<T>(Func<HttpServerResponse, T> actualFunc, T expected)
        {
            var response = _responses.Last();
            var actual = actualFunc(response);

            Assert.AreEqual(expected, actual);
            return this;
        }

        public FluentHttpServerTests AssertLastResponse<T, T1>(Func<T, T1> actualFunc, T1 expected) where T : IHttpHeader
        {
            var response = _responses.Last();
            var header = response.Headers.OfType<T>().First();
            var actual = actualFunc(header);

            Assert.AreEqual(expected, actual);
            return this;
        }

        public FluentHttpServerTests AssertLastResponse<T>(string expected) where T : IHttpHeader
        {
            return AssertLastResponse<T, string>(x => x.Value, expected);
        }

        public FluentHttpServerTests AssertLastResponseHasNoHeaderOf<T>() where T : IHttpHeader
        {
            var response = _responses.Last();
            var anyHeaderOfTypeT = response.Headers.OfType<T>().Any();            

            Assert.IsFalse(anyHeaderOfTypeT);
            return this;
        }

        public FluentHttpServerTests AssertRouteHandlerRequest<T>(Func<IHttpServerRequest, T> actualFunc, T expected)
        {
            var routeHandler = _routeHandlers.Single().Value;
            var request = routeHandler.Requests.Last();
            var actual = actualFunc(request);

            Assert.AreEqual(expected, actual);
            return this;
        }

        public FluentHttpServerTests AssertRouteHandlerReceivedRequest()
        {
            var routeHandler = _routeHandlers.Single().Value;
            Assert.AreEqual(1, routeHandler.Requests.Count());
            return this;
        }

        public FluentHttpServerTests AssertRouteHandlerReceivedNoRequests()
        {
            var routeHandler = _routeHandlers.Single().Value;
            Assert.AreEqual(0, routeHandler.Requests.Count());
            return this;
        }

        public FluentHttpServerTests AssertRouteHandlerRequest<T>(string prefix, Func<IHttpServerRequest, T> actualFunc, T expected)
        {
            var routeHandler = _routeHandlers[prefix];
            var request = routeHandler.Requests.Last();
            var actual = actualFunc(request);

            Assert.AreEqual(expected, actual);
            return this;
        }

        public FluentHttpServerTests AssertRouteHandlerReceivedRequest(string prefix)
        {
            var routeHandler = _routeHandlers[prefix];
            Assert.AreEqual(1, routeHandler.Requests.Count());
            return this;
        }

        public FluentHttpServerTests AssertRouteHandlerReceivedNoRequests(string prefix)
        {
            var routeHandler = _routeHandlers[prefix];
            Assert.AreEqual(0, routeHandler.Requests.Count());
            return this;
        }

        public FluentHttpServerTests CorsIsEnabled()
        {
            _configuration.EnableCors();
            _httpServer = new HttpServer(_configuration);
            return this;
        }

        public FluentHttpServerTests CorsIsEnabled(Action<ICorsConfiguration> builder)
        {
            _configuration.EnableCors(builder);
            _httpServer = new HttpServer(_configuration);
            return this;
        }
    }
}