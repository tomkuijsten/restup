using Devkoes.Restup.WebServer;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;

namespace WebServer.UnitTests
{
    [TestClass]
    public class RestWebServerTest
    {
        private string _basicGETAcceptXML =
@"GET /users/2 HTTP/1.1
Host: minwinpc:8800
Connection: Keep-Alive
Accept: text/xml";

        [TestMethod]
        public void HandleRequest_BasicGETAcceptXML_Status200WithXml()
        {
            var m = new RestWebServer();
            m.RegisterController<TestController>();
            var response = m.HandleRequest(_basicGETAcceptXML);

            Assert.AreEqual(response.BodyType, MediaType.XML);
            Assert.AreEqual(response.StatusCode, 200);
            StringAssert.Contains(response.Body, "<Name>Tom</Name>");
            StringAssert.Contains(response.Body, "<Age>30</Age>");
        }

        private string _basicPOST =
@"POST /users/2 HTTP/1.1
Host: minwinpc:8800
Connection: Keep-Alive";

        [TestMethod]
        public void HandleRequest_BasicPOST_LocationHeaderStatus201()
        {
            var m = new RestWebServer();
            m.RegisterController<TestController>();
            var response = m.HandleRequest(_basicPOST);

            Assert.AreEqual(response.StatusCode, 201);
            //Assert.AreEqual(response.Location, "/users/2");
        }
    }

    [RestController(InstanceCreationType.Singleton)]
    public class TestController
    {
        public class User
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        [UriFormat("/users/{userId}")]
        public GetResponse GetUser(int userId)
        {
            return new GetResponse(GetResponse.GetResponseStatus.OK, new User() { Name = "Tom", Age = 30 });
        }
    }
}
