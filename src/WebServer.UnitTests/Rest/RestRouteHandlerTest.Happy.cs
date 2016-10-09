using Microsoft.VisualStudio.TestTools.UnitTesting;
using Restup.HttpMessage;
using Restup.HttpMessage.Models.Schemas;
using Restup.Webserver.UnitTests.TestHelpers;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Restup.Webserver.UnitTests.Rest
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

        #region BasicGetWithUriPrefix
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

        #region BasicGetWithEscapedString
        private MutableHttpServerRequest _basicGETTextEncoding = new MutableHttpServerRequest()
        {
            Method = HttpMethod.GET,
            Uri = new Uri("/users/John%20Doe", UriKind.RelativeOrAbsolute),
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_BasicGETTextEncoding_DecodedStringResult()
        {
            var m = Utils.CreateRestRoutehandler<HappyPathTestTextEncodingController>();

            var response = await m.HandleRequest(_basicGETTextEncoding);

            StringAssert.Contains(response.ToString(), "John Doe");
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
        private readonly MutableHttpServerRequest _basicPOST = new MutableHttpServerRequest()
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

        [TestMethod]
        public async Task HandleRequest_BasicPOST_Status201()
        {
            var request = Utils.CreateHttpRequest(uri: new Uri("/userswithnolocation", UriKind.RelativeOrAbsolute), method: HttpMethod.POST);

            var m = Utils.CreateRestRoutehandler<HappyPathTestController>();
            var response = await m.HandleRequest(request);

            Assert.AreEqual(HttpResponseStatus.Created, response.ResponseStatus);
            Assert.AreEqual(null, response.Location);
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

            // constructor validation will execute the Func once so initial value is set at + 1
            StringAssert.Contains(content, "\"Name\":\"Johathan\"");
            StringAssert.Contains(content, "\"Age\":16");

            response = await m.HandleRequest(_basicControllerWithArgs);

            content = response.ToString();

            StringAssert.Contains(content, "\"Name\":\"Johathan\"");
            StringAssert.Contains(content, "\"Age\":17");
        }
        #endregion
    }
}

