using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Models.Schemas;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Devkoes.Restup.WebServer.UnitTests
{
    [TestClass]
    public class RestWebServerRainyDayTest
    {
        #region ConflictingPost
        private HttpRequest _conflictingPOST = new HttpRequest()
        {
            Method = HttpMethod.POST,
            Uri = new Uri("/users", UriKind.RelativeOrAbsolute),
            ResponseContentType = MediaType.JSON,
            Content = "{\"Name\": \"Tom\", \"Age\": 33}",
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_CreateWithExistingId_Conflicted()
        {
            var m = new RestWebServer();
            m.RegisterController<RaiyDayTestController>();
            var response = await m.HandleRequest(_conflictingPOST);

            StringAssert.Contains(response.Response, "409 Conflict");
            StringAssert.DoesNotMatch(response.Response, new Regex("Location:"));
        }
        #endregion

        #region MethodNotAllowed
        private HttpRequest _methodNotAllowedPUT = new HttpRequest()
        {
            Method = HttpMethod.DELETE,
            Uri = new Uri("/users", UriKind.RelativeOrAbsolute),
            ResponseContentType = MediaType.JSON,
            Content = "{\"Name\": \"Tom\", \"Age\": 33}",
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_BasicPUT_MethodNotAllowed()
        {
            var m = new RestWebServer();
            m.RegisterController<RaiyDayTestController>();
            var response = await m.HandleRequest(_methodNotAllowedPUT);

            StringAssert.Contains(response.Response, "405 Method Not Allowed");
            StringAssert.Contains(response.Response, "Allow: POST");
        }
        #endregion

        #region ParameterParseException
        private HttpRequest _parameterParseExceptionPUT = new HttpRequest()
        {
            Method = HttpMethod.PUT,
            Uri = new Uri("/users/notanumber", UriKind.RelativeOrAbsolute),
            ResponseContentType = MediaType.JSON,
            Content = "{\"Name\": \"Tom\", \"Age\": 33}",
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_WrongParameterTypePUT_BadRequest()
        {
            var m = new RestWebServer();
            m.RegisterController<RaiyDayTestController>();
            var response = await m.HandleRequest(_parameterParseExceptionPUT);

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

        #region JsonBodyParameterValueParseException
        private HttpRequest _bodyParameterParseExPOST = new HttpRequest()
        {
            Method = HttpMethod.POST,
            Uri = new Uri("/users", UriKind.RelativeOrAbsolute),
            ResponseContentType = MediaType.JSON,
            Content = "{\"Name\": \"Tom\", \"Age\": notanumber}",
            IsComplete = true
        };


        [TestMethod]
        public async Task HandleRequest_InvalidJSONBodyParameter_BadRequest()
        {
            var m = new RestWebServer();
            m.RegisterController<RaiyDayTestController>();
            var response = await m.HandleRequest(_bodyParameterParseExPOST);

            StringAssert.Contains(response.Response, "400 Bad Request");
        }
        #endregion

        #region XmlBodyParameterValueParseException
        private HttpRequest _xmlBodyParameterParseExPOST = new HttpRequest()
        {
            Method = HttpMethod.POST,
            Uri = new Uri("/users", UriKind.RelativeOrAbsolute),
            ResponseContentType = MediaType.JSON,
            Content = "<User><Name>Tom</Name><Age>thirtythree</Age></User>",
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_InvalidXMLBodyParameter_BadRequest()
        {
            var m = new RestWebServer();
            m.RegisterController<RaiyDayTestController>();
            var response = await m.HandleRequest(_xmlBodyParameterParseExPOST);

            StringAssert.Contains(response.Response, "400 Bad Request");
        }
        #endregion

        #region InvalidJsonFormatParseException
        private HttpRequest _invalidJsonFormatPOST = new HttpRequest()
        {
            Method = HttpMethod.POST,
            Uri = new Uri("/users", UriKind.RelativeOrAbsolute),
            ResponseContentType = MediaType.JSON,
            Content = "{\"Name\": \"Tom\"; \"Age\": 33}",
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_InvalidJsonFormat_BadRequest()
        {
            var m = new RestWebServer();
            m.RegisterController<RaiyDayTestController>();
            var response = await m.HandleRequest(_invalidJsonFormatPOST);

            StringAssert.Contains(response.Response, "400 Bad Request");
        }
        #endregion

        #region InvalidXmlFormatParseException
        private HttpRequest _invalidXmlFormatExPOST = new HttpRequest()
        {
            Method = HttpMethod.POST,
            Uri = new Uri("/users", UriKind.RelativeOrAbsolute),
            ResponseContentType = MediaType.JSON,
            Content = "<User><Name>Tom</><Age>thirtythree</Age></User>",
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_InvalidJsonBodyParameter_BadRequest()
        {
            var m = new RestWebServer();
            m.RegisterController<RaiyDayTestController>();
            var response = await m.HandleRequest(_invalidXmlFormatExPOST);

            StringAssert.Contains(response.Response, "400 Bad Request");
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

        [UriFormat("/users")]
        public PostResponse CreateUser([FromBody] User user)
        {
            return new PostResponse(PostResponse.ResponseStatus.Conflict);
        }

        [UriFormat("/users/{id}")]
        public PutResponse UpdateUser(int id, [FromBody] User user)
        {
            return new PutResponse(PutResponse.ResponseStatus.OK);
        }
    }

    [RestController(InstanceCreationType.Singleton)]
    public class ParameterTypeErrorTestController
    {
        [UriFormat("/users")]
        public PostResponse CreateUser(object id)
        {
            return new PostResponse(PostResponse.ResponseStatus.Conflict);
        }
    }
}
