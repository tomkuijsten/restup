using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Restup.HttpMessage;
using Restup.HttpMessage.Models.Contracts;
using Restup.HttpMessage.Models.Schemas;
using Restup.Webserver.Http;
using Restup.Webserver.Models.Contracts;

namespace Restup.Webserver.UnitTests.TestHelpers
{
    public class FluentHttpServerTests : FluentHttpServerTests<FluentHttpServerTests> { }

    public class FluentHttpServerTests<T> where T : FluentHttpServerTests<T>
    {
        private HttpServer _httpServer;
        private readonly List<HttpServerResponse> _responses;

        private readonly Dictionary<string, EchoRouteHandler> _routeHandlers;
        private readonly HttpServerConfiguration _configuration;

        public T Given => (T) this;
        public T When => (T) this;
        public T Then => (T) this;

        public FluentHttpServerTests()
        {
            _configuration = new HttpServerConfiguration();
            _httpServer = new HttpServer(_configuration);
            _responses = new List<HttpServerResponse>();
            _routeHandlers = new Dictionary<string, EchoRouteHandler>();
        }

        public T ListeningOnDefaultRoute()
        {
            var routeHandler = new EchoRouteHandler();
            _routeHandlers[string.Empty] = routeHandler;
            _configuration.RegisterRoute(routeHandler);
            _httpServer = new HttpServer(_configuration);
            return (T) this;
        }

        public T ListeningOnRoute(string urlPrefix)
        {
            var routeHandler = new EchoRouteHandler();
            _routeHandlers[urlPrefix] = routeHandler;
            RegisterRouteHandler(urlPrefix, routeHandler);
            return (T) this;
        }

        protected void RegisterRouteHandler(string urlPrefix, IRouteHandler routeHandler)
        {
            _configuration.RegisterRoute(urlPrefix, routeHandler);
            _httpServer = new HttpServer(_configuration);
        }

        public T RequestHasArrived(string uri, IEnumerable<string> acceptEncodings = null,
            byte[] content = null, string origin = null, HttpMethod? method = HttpMethod.GET, HttpMethod? accessControlRequestMethod = null,
            IEnumerable<string> accessControlRequestHeaders = null, string contentType = null, IEnumerable<string> acceptCharsets = null,
            string contentCharset = null, IEnumerable<string> acceptMediaTypes = null)
        {
            var httpServerRequest = Utils.CreateHttpRequest(uri: new Uri(uri, UriKind.Relative),
                acceptEncodings: acceptEncodings, content: content, origin: origin, method: method,
                accessControlRequestMethod: accessControlRequestMethod, accessControlRequestHeaders: accessControlRequestHeaders,
                contentType: contentType, acceptCharsets: acceptCharsets, contentTypeCharset: contentCharset, acceptMediaTypes: acceptMediaTypes);
            var response = _httpServer.HandleRequestAsync(httpServerRequest).Result;
            _responses.Add(response);
            return (T) this;
        }

        public T AssertLastResponse<TValue>(Func<HttpServerResponse, TValue> actualFunc, TValue expected)
        {
            var response = _responses.Last();
            var actual = actualFunc(response);

            Assert.AreEqual(expected, actual);
            return (T) this;
        }

        public T AssertLastResponseContent(string expected)
        {
            var response = _responses.Last();
            var encoding = Encoding.GetEncoding(response.ContentCharset);
            var actual = encoding.GetString(response.Content);

            Assert.AreEqual(expected, actual);
            return (T)this;
        }

        public T AssertLastResponse<THeader, TValue>(Func<THeader, TValue> actualFunc, TValue expected) where THeader : IHttpHeader
        {
            var response = _responses.Last();
            var header = response.Headers.OfType<THeader>().First();
            var actual = actualFunc(header);

            Assert.AreEqual(expected, actual);
            return (T) this;
        }

        public T AssertLastResponse<THeader>(string expected) where THeader : IHttpHeader
        {
            return AssertLastResponse<THeader, string>(x => x.Value, expected);
        }

        public T AssertLastResponseHasNoHeaderOf<THeader>() where THeader : IHttpHeader
        {
            var response = _responses.Last();
            var anyHeaderOfTypeT = response.Headers.OfType<THeader>().Any();

            Assert.IsFalse(anyHeaderOfTypeT);
            return (T) this;
        }

        public T AssertRouteHandlerRequest<TValue>(Func<IHttpServerRequest, TValue> actualFunc, TValue expected)
        {
            var routeHandler = _routeHandlers.Single().Value;
            var request = routeHandler.Requests.Last();
            var actual = actualFunc(request);

            Assert.AreEqual(expected, actual);
            return (T) this;
        }

        public T AssertRouteHandlerReceivedRequest()
        {
            var routeHandler = _routeHandlers.Single().Value;
            Assert.AreEqual(1, routeHandler.Requests.Count());
            return (T) this;
        }

        public T AssertRouteHandlerReceivedNoRequests()
        {
            var routeHandler = _routeHandlers.Single().Value;
            Assert.AreEqual(0, routeHandler.Requests.Count());
            return (T) this;
        }

        public T AssertRouteHandlerRequest<TValue>(string prefix, Func<IHttpServerRequest, TValue> actualFunc, TValue expected)
        {
            var routeHandler = _routeHandlers[prefix];
            var request = routeHandler.Requests.Last();
            var actual = actualFunc(request);

            Assert.AreEqual(expected, actual);
            return (T) this;
        }

        public T AssertRouteHandlerReceivedRequest(string prefix)
        {
            var routeHandler = _routeHandlers[prefix];
            Assert.AreEqual(1, routeHandler.Requests.Count());
            return (T) this;
        }

        public T AssertRouteHandlerReceivedNoRequests(string prefix)
        {
            var routeHandler = _routeHandlers[prefix];
            Assert.AreEqual(0, routeHandler.Requests.Count());
            return (T) this;
        }

        public T CorsIsEnabled()
        {
            _configuration.EnableCors();
            _httpServer = new HttpServer(_configuration);
            return (T) this;
        }

        public T CorsIsEnabled(Action<ICorsConfiguration> builder)
        {
            _configuration.EnableCors(builder);
            _httpServer = new HttpServer(_configuration);
            return (T) this;
        }
    }
}