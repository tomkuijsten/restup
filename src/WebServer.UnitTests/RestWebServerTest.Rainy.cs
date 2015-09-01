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
        private string _basicPOST =
@"POST /users/1 HTTP/1.1
Host: minwinpc:8800
Content-Type: application/json

{""Name"": ""Tom"", ""Age"": 33}";

        [TestMethod]
        public void HandleRequest_BasicPOST_ConlictedUser()
        {
            var m = new RestWebServer();
            m.RegisterController<RaiyDayTestController>();
            var response = m.HandleRequest(_basicPOST);

            StringAssert.Contains(response.Response, "409 Conflict");
            StringAssert.DoesNotMatch(response.Response, new Regex("Location:"));
        }

        [TestMethod]
        public void HandleRequest_BasicPOST_NotFoundUser()
        {
            var m = new RestWebServer();
            m.RegisterController<RaiyDayTestController>();
            var response = m.HandleRequest(_basicPOST);

            StringAssert.Contains(response.Response, "405 MethodNotAllowed");
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

        [UriFormat("/users/{userId}")]
        public GetResponse GetUser(int userId)
        {
            return new GetResponse(GetResponse.ResponseStatus.OK, new User() { Name = "Tom", Age = 30 });
        }

        [UriFormat("/users/{id}")]
        public PostResponse CreateUser(int id, [FromBody] User user)
        {
            if(id == 1)
                return new PostResponse(PostResponse.ResponseStatus.Conflict);
            else
                return new PostResponse(PostResponse.ResponseStatus.MethodNotFound);
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
