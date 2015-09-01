using Devkoes.Restup.WebServer;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Text.RegularExpressions;

namespace WebServer.UnitTests
{
	[TestClass]
    public class RestWebServerRainyDayTest
    {
        #region BasicPost
        private string _conflictingPOST =
@"POST /users/1 HTTP/1.1
Host: minwinpc:8800
Content-Type: application/json

{""Name"": ""Tom"", ""Age"": 33}";

        [TestMethod]
        public void HandleRequest_BasicPOST_Conflicted()
        {
            var m = new RestWebServer();
            m.RegisterController<RaiyDayTestController>();
            var response = m.HandleRequest(_conflictingPOST);

            StringAssert.Contains(response.Response, "409 Conflict");
            StringAssert.DoesNotMatch(response.Response, new Regex("Location:"));
        }

        private string _methodNotAllowedPOST =
@"POST /users/2 HTTP/1.1
Host: minwinpc:8800
Content-Type: application/json

{""Name"": ""Tom"", ""Age"": 33}";

        [TestMethod]
        public void HandleRequest_BasicPOST_MethodNotAllowed()
        {
            var m = new RestWebServer();
            m.RegisterController<RaiyDayTestController>();
            var response = m.HandleRequest(_methodNotAllowedPOST);

            StringAssert.Contains(response.Response, "405 Method Not Allowed");
            StringAssert.DoesNotMatch(response.Response, new Regex("Location:"));
        }
        #endregion
    }

    [RestController(InstanceCreationType.Singleton)]
    public class RaiyDayTestController
    {
        public class User
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        [UriFormat("/users/{id}")]
        public PostResponse CreateUser(int id, [FromBody] User user)
        {
            if(id == 1)
                return new PostResponse(PostResponse.ResponseStatus.Conflict);
            else
                return new PostResponse(PostResponse.ResponseStatus.MethodNotFound);
        }
    }
}
