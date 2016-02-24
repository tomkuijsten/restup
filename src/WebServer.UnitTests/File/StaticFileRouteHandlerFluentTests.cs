using System;
using System.Text;
using Devkoes.HttpMessage;
using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.Restup.WebServer.File;
using Devkoes.Restup.WebServer.UnitTests.TestHelpers;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Devkoes.Restup.WebServer.UnitTests.File
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
            routeHandler = new StaticFileRouteHandler(basePath, mockFileSystem);

            return this;
        }

        public StaticFileRouteHandlerFluentTests FileExists(string filePath, string content = null, string contentType = "utf-8")
        {
            var mockFile = new MockFile(content ?? string.Empty, contentType);
            mockFileSystem.AddFile(filePath, mockFile);

            return this;
        }

        public StaticFileRouteHandlerFluentTests GetRequestReceived(string uri)
        {
            var request = Utils.CreateHttpRequest(uri: new Uri(uri, UriKind.Relative));
            response = routeHandler.HandleRequest(request).Result;
            
            return this;
        }

        public StaticFileRouteHandlerFluentTests AssertResponse<T>(Func<HttpServerResponse, T> actualValueFunc, T expectedValue)
        {
            actualValueFunc(response);
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

        public StaticFileRouteHandlerFluentTests AssertResponseContent(string content)
        {
            var encoding = Encoding.GetEncoding(response.ContentType);
            var contentAsBytes = encoding.GetBytes(content);

            CollectionAssert.AreEqual(contentAsBytes, response.Content);
            return this;
        }
    }
}