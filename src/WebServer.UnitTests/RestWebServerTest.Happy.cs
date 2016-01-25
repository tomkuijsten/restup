using Devkoes.HttpMessage;
using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.Restup.WebServer.UnitTests.TestHelpers;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Devkoes.Restup.WebServer.UnitTests
{
    [TestClass]
    public class RestWebServerHappyPathTest
    {
        #region BasicGetAcceptXML
        private HttpServerRequest _basicGETAcceptXML = new HttpServerRequest()
        {
            Method = HttpMethod.GET,
            Uri = new Uri("/users/2", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { MediaType.XML },
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_BasicGETAcceptXML_Status200WithXml()
        {
            var m = new RestWebServer();
            m.RegisterController<HappyPathTestController>();
            var response = await m.HandleRequest(_basicGETAcceptXML);

            StringAssert.Contains(response.Response, "200 OK");
            StringAssert.Contains(response.Response, "Content-Type: application/xml");
            StringAssert.Contains(response.Response, "<Name>Tom</Name>");
            StringAssert.Contains(response.Response, "<Age>30</Age>");
        }
        #endregion

        #region BasicGetWithAbsoluteUri
        private HttpServerRequest _basicGETAbsoluteUri = new HttpServerRequest()
        {
            Method = HttpMethod.GET,
            Uri = new Uri("http://myserverx:1234/users/2", UriKind.RelativeOrAbsolute),
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_BasicGETAbsoluteUri_Status200()
        {
            var m = new RestWebServer();
            m.RegisterController<HappyPathTestController>();
            var response = await m.HandleRequest(_basicGETAbsoluteUri);

            StringAssert.Contains(response.Response, "200 OK");
        }
        #endregion

        #region BasicGetWithAbsoluteUri
        private HttpServerRequest _basicGETUriPrefix = new HttpServerRequest()
        {
            Method = HttpMethod.GET,
            Uri = new Uri("api/users/2", UriKind.RelativeOrAbsolute),
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_BasicGETUriPrefix_Status200()
        {
            var m = new RestWebServer(8800, "api");
            m.RegisterController<HappyPathTestController>();
            var response = await m.HandleRequest(_basicGETUriPrefix);

            StringAssert.Contains(response.Response, "200 OK");
        }
        #endregion

        #region BasicGetAcceptJSON
        private HttpServerRequest _basicGETAcceptJSON = new HttpServerRequest()
        {
            Method = HttpMethod.GET,
            Uri = new Uri("/users/2", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { MediaType.JSON },
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_BasicGETAcceptJSON_Status200WithJSON()
        {
            var m = new RestWebServer();
            m.RegisterController<HappyPathTestController>();
            var response = await m.HandleRequest(_basicGETAcceptJSON);

            StringAssert.Contains(response.Response, "200 OK");
            StringAssert.Contains(response.Response, "Content-Type: application/json");
            StringAssert.Contains(response.Response, "\"Name\":\"Tom\"");
            StringAssert.Contains(response.Response, "\"Age\":30");
        }
        #endregion

        #region BasicPost
        private HttpServerRequest _basicPOST = new HttpServerRequest()
        {
            Method = HttpMethod.POST,
            Uri = new Uri("/users", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { MediaType.JSON },
            Content = Encoding.UTF8.GetBytes("{\"Name\": \"Tom\", \"Age\": 33}"),
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_BasicPOST_LocationHeaderStatus201()
        {
            var m = new RestWebServer();
            m.RegisterController<HappyPathTestController>();
            var response = await m.HandleRequest(_basicPOST);

            StringAssert.Contains(response.Response, "201 Created");
            StringAssert.Contains(response.Response, "Location: /users/2");
        }
        #endregion

        #region BasicPut
        private HttpServerRequest _basicPUT = new HttpServerRequest()
        {
            Method = HttpMethod.PUT,
            Uri = new Uri("/users/2", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { MediaType.JSON },
            Content = Encoding.UTF8.GetBytes("{Name: Tom, Age: 21}"),
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_BasicPUT_Status200()
        {
            var m = new RestWebServer();
            m.RegisterController<HappyPathTestController>();
            var response = await m.HandleRequest(_basicPUT);

            StringAssert.Contains(response.Response, "200 OK");
        }
        #endregion

        #region BasicDelete
        private HttpServerRequest _basicDEL = new HttpServerRequest()
        {
            Method = HttpMethod.DELETE,
            Uri = new Uri("/users/2", UriKind.RelativeOrAbsolute),
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_BasicDEL_Status200()
        {
            var m = new RestWebServer();
            m.RegisterController<HappyPathTestController>();
            var response = await m.HandleRequest(_basicDEL);

            StringAssert.Contains(response.Response, "200 OK");
        }
        #endregion

        #region RestControllerWithArgs
        private HttpServerRequest _basicControllerWithArgs = new HttpServerRequest()
        {
            Method = HttpMethod.GET,
            Uri = new Uri("/users/2", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { MediaType.JSON },
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_ControllerWithArgs_MatchConstructor()
        {
            var m = new RestWebServer();
            m.RegisterController<HappyPathTestSingletonControllerWithArgs>("Johathan", 15);
            var response = await m.HandleRequest(_basicControllerWithArgs);

            StringAssert.Contains(response.Response, "\"Name\":\"Johathan\"");
            StringAssert.Contains(response.Response, "\"Age\":15");
        }

        [TestMethod]
        public async Task HandleRequest_PerCallControllerWithArgs_MatchConstructor()
        {
            int age = 15;
            var m = new RestWebServer();
            m.RegisterController<HappyPathTestPerCallControllerWithArgs>(() => new object[] { "Johathan", age++ });
            var response = await m.HandleRequest(_basicControllerWithArgs);

            StringAssert.Contains(response.Response, "\"Name\":\"Johathan\"");
            StringAssert.Contains(response.Response, "\"Age\":15");

            response = await m.HandleRequest(_basicControllerWithArgs);

            StringAssert.Contains(response.Response, "\"Name\":\"Johathan\"");
            StringAssert.Contains(response.Response, "\"Age\":16");
        }
        #endregion
    }
}

