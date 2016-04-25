using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Restup.HttpMessage;
using Restup.HttpMessage.Models.Schemas;
using Restup.Webserver.UnitTests.TestHelpers;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Restup.Webserver.UnitTests
{
    [TestClass]
    public class RestRouteHandlerHappyPathTest
    {
        #region BasicGetAcceptXML
        private MutableHttpServerRequest _basicGETAcceptXML = new MutableHttpServerRequest()
        {
            Method = HttpMethod.GET,
            Uri = new Uri("/users/2", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { "application/xml" },
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_BasicGETAcceptXML_Status200WithXml()
        {
            var m = Utils.CreateRestRoutehandler<HappyPathTestController>();

            var response = await m.HandleRequest(_basicGETAcceptXML);

            string content = response.ToString();

            StringAssert.Contains(content, "200 OK");
            StringAssert.Contains(content, "Content-Type: application/xml");
            StringAssert.Contains(content, "<Name>Tom</Name>");
            StringAssert.Contains(content, "<Age>30</Age>");
        }
        #endregion

        #region BasicGetWithParamAcceptXML
        private MutableHttpServerRequest _basicGETWithParamAcceptXML = new MutableHttpServerRequest()
        {
            Method = HttpMethod.GET,
            Uri = new Uri("/users?userId=2", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { "application/xml" },
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_BasicGETWithParamAcceptXML_Status200WithXml()
        {
            var m = Utils.CreateRestRoutehandler<HappyPathTestController>();

            var response = await m.HandleRequest(_basicGETWithParamAcceptXML);

            string content = response.ToString();

            StringAssert.Contains(content, "200 OK");
            StringAssert.Contains(content, "Content-Type: application/xml");
            StringAssert.Contains(content, "<Name>Tom</Name>");
            StringAssert.Contains(content, "<Age>30</Age>");
        }
        #endregion

        #region BasicGetWithAbsoluteUri
        private MutableHttpServerRequest _basicGETAbsoluteUri = new MutableHttpServerRequest()
        {
            Method = HttpMethod.GET,
            Uri = new Uri("http://myserverx:1234/users/2", UriKind.RelativeOrAbsolute),
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_BasicGETAbsoluteUri_Status200()
        {
            var m = Utils.CreateRestRoutehandler<HappyPathTestController>();

            var response = await m.HandleRequest(_basicGETAbsoluteUri);

            StringAssert.Contains(response.ToString(), "200 OK");
        }
        #endregion

        #region BasicGetWithAbsoluteUri
        private MutableHttpServerRequest _basicGETUriPrefix = new MutableHttpServerRequest()
        {
            Method = HttpMethod.GET,
            Uri = new Uri("/users/2", UriKind.RelativeOrAbsolute),
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_BasicGETUriPrefix_Status200()
        {
            var m = Utils.CreateRestRoutehandler<HappyPathTestController>();

            var response = await m.HandleRequest(_basicGETUriPrefix);

            StringAssert.Contains(response.ToString(), "200 OK");
        }
        #endregion

        #region BasicGetAcceptJSON
        private MutableHttpServerRequest _basicGETAcceptJSON = new MutableHttpServerRequest()
        {
            Method = HttpMethod.GET,
            Uri = new Uri("/users/2", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { "text/json" },
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_BasicGETAcceptJSON_Status200WithJSON()
        {
            var m = Utils.CreateRestRoutehandler<HappyPathTestController>();

            var response = await m.HandleRequest(_basicGETAcceptJSON);

            string content = response.ToString();

            StringAssert.Contains(content, "200 OK");
            StringAssert.Contains(content, "Content-Type: application/json");
            StringAssert.Contains(content, "\"Name\":\"Tom\"");
            StringAssert.Contains(content, "\"Age\":30");
        }
        #endregion

        #region BasicPost
        private MutableHttpServerRequest _basicPOST = new MutableHttpServerRequest()
        {
            Method = HttpMethod.POST,
            Uri = new Uri("/users", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { "application/json" },
            Content = Encoding.UTF8.GetBytes("{\"Name\": \"Tom\", \"Age\": 33}"),
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_BasicPOST_LocationHeaderStatus201()
        {
            var m = Utils.CreateRestRoutehandler<HappyPathTestController>();

            var response = await m.HandleRequest(_basicPOST);

            string content = response.ToString();

            StringAssert.Contains(content, "201 Created");
            StringAssert.Contains(content, "Location: /users/2");
        }
        #endregion

        #region BasicPut
        private MutableHttpServerRequest _basicPUT = new MutableHttpServerRequest()
        {
            Method = HttpMethod.PUT,
            Uri = new Uri("/users/2", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { "application/json" },
            Content = Encoding.UTF8.GetBytes("{Name: Tom, Age: 21}"),
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_BasicPUT_Status200()
        {
            var m = Utils.CreateRestRoutehandler<HappyPathTestController>();

            var response = await m.HandleRequest(_basicPUT);

            StringAssert.Contains(response.ToString(), "200 OK");
        }
        #endregion

        #region BasicDelete
        private MutableHttpServerRequest _basicDEL = new MutableHttpServerRequest()
        {
            Method = HttpMethod.DELETE,
            Uri = new Uri("/users/2", UriKind.RelativeOrAbsolute),
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_BasicDEL_Status200()
        {
            var m = Utils.CreateRestRoutehandler<HappyPathTestController>();

            var response = await m.HandleRequest(_basicDEL);

            StringAssert.Contains(response.ToString(), "200 OK");
        }
        #endregion

        #region RestControllerWithArgs
        private MutableHttpServerRequest _basicControllerWithArgs = new MutableHttpServerRequest()
        {
            Method = HttpMethod.GET,
            Uri = new Uri("/users/2", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { "text/json" },
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_ControllerWithArgs_MatchConstructor()
        {
            var m = Utils.CreateRestRoutehandler<HappyPathTestSingletonControllerWithArgs>("Johathan", 15);

            var response = await m.HandleRequest(_basicControllerWithArgs);

            string content = response.ToString();

            StringAssert.Contains(content, "\"Name\":\"Johathan\"");
            StringAssert.Contains(content, "\"Age\":15");
        }

        [TestMethod]
        public async Task HandleRequest_PerCallControllerWithArgs_MatchConstructor()
        {
            int age = 15;
            var m = Utils.CreateRestRoutehandler<HappyPathTestPerCallControllerWithArgs>(() => new object[] { "Johathan", age++ });

            var response = await m.HandleRequest(_basicControllerWithArgs);

            string content = response.ToString();

            StringAssert.Contains(content, "\"Name\":\"Johathan\"");
            StringAssert.Contains(content, "\"Age\":15");

            response = await m.HandleRequest(_basicControllerWithArgs);

            content = response.ToString();

            StringAssert.Contains(content, "\"Name\":\"Johathan\"");
            StringAssert.Contains(content, "\"Age\":16");
        }
        #endregion
    }
}

