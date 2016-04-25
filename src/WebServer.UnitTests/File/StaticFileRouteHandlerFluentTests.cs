using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Restup.HttpMessage;
using Restup.HttpMessage.Models.Schemas;
using Restup.Webserver.File;
using Restup.Webserver.UnitTests.TestHelpers;
using System;
using System.Text;

namespace Restup.Webserver.UnitTests.File
{
    public class StaticFileRouteHandlerFluentTests
    {
        private StaticFileRouteHandler routeHandler;
        private MockFileSystem mockFileSystem;
        private HttpServerResponse response;

        public StaticFileRouteHandlerFluentTests Given => this;
        public StaticFileRouteHandlerFluentTests When => this;
        public StaticFileRouteHandlerFluentTests Then => this;

        public StaticFileRouteHandlerFluentTests SetUp(string basePath, bool pathExists = true)
        {
            mockFileSystem = new MockFileSystem(pathExists);
            routeHandler = new StaticFileRouteHandler(basePath, null, mockFileSystem);

            return this;
        }

        public StaticFileRouteHandlerFluentTests FileExists(string filePath, string content = null, string extension = ".html", string contentType = "text/html")
        {
            var mockFile = new MockFile(content ?? string.Empty, contentType, extension);
            mockFileSystem.AddFile(filePath, mockFile);

            return this;
        }

        public StaticFileRouteHandlerFluentTests GetRequestReceived(string uri, HttpMethod method = HttpMethod.GET)
        {
            var request = Utils.CreateHttpRequest(uri: new Uri(uri, UriKind.Relative), method: method);
            response = routeHandler.HandleRequest(request).Result;

            return this;
        }

        public StaticFileRouteHandlerFluentTests AssertResponse<T>(Func<HttpServerResponse, T> actualValueFunc, T expectedValue)
        {
            var actual = actualValueFunc(response);
            Assert.AreEqual(expectedValue, actual);
            return this;
        }

        public StaticFileRouteHandlerFluentTests AssertOkResponseExists()
        {
            Assert.IsNotNull(response);
            Assert.AreEqual(HttpResponseStatus.OK, response.ResponseStatus);
            return this;
        }

        public StaticFileRouteHandlerFluentTests AssertFileNotFoundResponseExists()
        {
            Assert.IsNotNull(response);
            Assert.AreEqual(HttpResponseStatus.NotFound, response.ResponseStatus);
            return this;
        }

        public StaticFileRouteHandlerFluentTests AssertMethodNotAllowedResponseExists()
        {
            Assert.IsNotNull(response);
            Assert.AreEqual(HttpResponseStatus.MethodNotAllowed, response.ResponseStatus);
            return this;
        }

        public StaticFileRouteHandlerFluentTests AssertResponseContent(string content)
        {
            var contentAsBytes = Encoding.UTF8.GetBytes(content);

            CollectionAssert.AreEqual(contentAsBytes, response.Content);
            return this;
        }
    }
}