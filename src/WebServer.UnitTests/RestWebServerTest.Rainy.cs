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
        #region ConflictingPost
        private string _conflictingPOST =
@"POST /users/1 HTTP/1.1
Host: minwinpc:8800
Content-Type: application/json

{""Name"": ""Tom"", ""Age"": 33}";

        [TestMethod]
        public void HandleRequest_CreateWithExistingId_Conflicted()
        {
            var m = new RestWebServer();
            m.RegisterController<RaiyDayTestController>();
            var response = m.HandleRequest(_conflictingPOST);

            StringAssert.Contains(response.Response, "409 Conflict");
            StringAssert.DoesNotMatch(response.Response, new Regex("Location:"));
        }
        #endregion

        #region MethodNotAllowed
        private string _methodNotAllowedPUT =
@"PUT /users/2 HTTP/1.1
Host: minwinpc:8800
Content-Type: application/json

{""Name"": ""Tom"", ""Age"": 33}";

        [TestMethod]
        public void HandleRequest_BasicPUT_MethodNotAllowed()
        {
            var m = new RestWebServer();
            m.RegisterController<RaiyDayTestController>();
            var response = m.HandleRequest(_methodNotAllowedPUT);

            StringAssert.Contains(response.Response, "405 Method Not Allowed");
            StringAssert.Contains(response.Response, "Allow: POST");
        }
        #endregion

        #region ParameterParseException
        private string _parameterParseExceptionPUT =
@"POST /users/notanumber HTTP/1.1
Host: minwinpc:8800
Content-Type: application/json

{""Name"": ""Tom"", ""Age"": 33}";

        [TestMethod]
        public void HandleRequest_WrongParameterTypePUT_BadRequest()
        {
            var m = new RestWebServer();
            m.RegisterController<RaiyDayTestController>();
            var response = m.HandleRequest(_parameterParseExceptionPUT);

            StringAssert.Contains(response.Response, "400 Bad Request");
        }
        #endregion

        #region ParameterTypeException
        [TestMethod]
        public void HandleRequest_WrongParameterTypeInController_InvalidOperationException()
        {
            var m = new RestWebServer();

            bool invOpThrown = false;
            try
            {
                m.RegisterController<ParameterTypeErrorTestController>();
            }
            catch (InvalidOperationException)
            {
                invOpThrown = true;
            }

            Assert.IsTrue(invOpThrown, "InvalidOperationException was not thrown");
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
            return new PostResponse(PostResponse.ResponseStatus.Conflict);
        }
    }

    [RestController(InstanceCreationType.Singleton)]
    public class ParameterTypeErrorTestController
    {
        [UriFormat("/users/{id}")]
        public PostResponse CreateUser(object id)
        {
            return new PostResponse(PostResponse.ResponseStatus.Conflict);
        }
    }
}
