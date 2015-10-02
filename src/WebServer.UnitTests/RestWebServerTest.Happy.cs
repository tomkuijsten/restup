using Devkoes.Restup.WebServer;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Threading.Tasks;

namespace WebServer.UnitTests
{
    [TestClass]
    public class RestWebServerHappyPathTest
    {
        #region BasicGetAcceptXML
        private string _basicGETAcceptXML =
@"GET /users/2 HTTP/1.1
Host: minwinpc:8800
Accept: text/xml";

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

        #region BasicGetAcceptJSON
        private string _basicGETAcceptJSON =
@"GET /users/2 HTTP/1.1
Host: minwinpc:8800
Accept: application/json";

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
        private string _basicPOST =
@"POST /users HTTP/1.1
Host: minwinpc:8800
Content-Type: application/json

{""Name"": ""Tom"", ""Age"": 33}";

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
        private string _basicPUT =
@"PUT /users/2 HTTP/1.1
Host: minwinpc:8800
Content-Type: application/json

{Name: Tom, Age: 21}";

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
        private string _basicDEL =
@"DELETE /users/2 HTTP/1.1
Host: minwinpc:8800";

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
