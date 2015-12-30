using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Models.Schemas;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Threading.Tasks;

namespace Devkoes.Restup.WebServer.UnitTests
{
    [TestClass]
    public class RestWebServerHappyPathTest
    {
        #region BasicGetAcceptXML
        private HttpRequest _basicGETAcceptXML = new HttpRequest()
        {
            Method = HttpMethod.GET,
            Uri = new Uri("/users/2", UriKind.RelativeOrAbsolute),
            ResponseContentType = MediaType.XML,
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
        private HttpRequest _basicGETAbsoluteUri = new HttpRequest()
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
        private HttpRequest _basicGETUriPrefix = new HttpRequest()
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
        private HttpRequest _basicGETAcceptJSON = new HttpRequest()
        {
            Method = HttpMethod.GET,
            Uri = new Uri("/users/2", UriKind.RelativeOrAbsolute),
            ResponseContentType = MediaType.JSON,
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
        private HttpRequest _basicPOST = new HttpRequest()
        {
            Method = HttpMethod.POST,
            Uri = new Uri("/users", UriKind.RelativeOrAbsolute),
            ResponseContentType = MediaType.JSON,
            Content = "{\"Name\": \"Tom\", \"Age\": 33}",
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
        private HttpRequest _basicPUT = new HttpRequest()
        {
            Method = HttpMethod.PUT,
            Uri = new Uri("/users/2", UriKind.RelativeOrAbsolute),
            ResponseContentType = MediaType.JSON,
            Content = "{Name: Tom, Age: 21}",
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
        private HttpRequest _basicDEL = new HttpRequest()
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
    }

    [RestController(InstanceCreationType.Singleton)]
    public class HappyPathTestController
    {
        public class User
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        [UriFormat("/users/{userId}")]
        public GetResponse GetUser(int userId)
        {
            return new GetResponse(GetResponse.ResponseStatus.OK, new User() { Name = "Tom", Age = 30 });
        }

        [UriFormat("/users")]
        public PostResponse CreateUser([FromBody] User user)
        {
            return new PostResponse(PostResponse.ResponseStatus.Created, $"/users/2");
        }

        [UriFormat("/users/{userId}")]
        public PutResponse UpdateUser(int userId)
        {
            return new PutResponse(PutResponse.ResponseStatus.OK);
        }

        [UriFormat("/users/{userId}")]
        public DeleteResponse DeleteUser(int userId)
        {
            return new DeleteResponse(DeleteResponse.ResponseStatus.OK);
        }
    }
}
