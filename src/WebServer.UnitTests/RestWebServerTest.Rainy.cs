using Devkoes.HttpMessage;
using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Devkoes.Restup.WebServer.UnitTests
{
    [TestClass]
    public class RestWebServerRainyDayTest
    {
        #region ConflictingPost
        private HttpServerRequest _conflictingPOST = new HttpServerRequest()
        {
            Method = HttpMethod.POST,
            Uri = new Uri("/users", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { MediaType.JSON },
            Content = Encoding.UTF8.GetBytes("{\"Name\": \"Tom\", \"Age\": 33}"),
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_CreateWithExistingId_Conflicted()
        {
            var m = new RestWebServer();
            m.RegisterController<RaiyDayTestController>();
            var response = await m.HandleRequest(_conflictingPOST);

            string content = response.ToString();

            StringAssert.Contains(content, "409 Conflict");
            StringAssert.DoesNotMatch(content, new Regex("Location:"));
        }
        #endregion

        #region MethodNotAllowed
        private HttpServerRequest _methodNotAllowedPUT = new HttpServerRequest()
        {
            Method = HttpMethod.DELETE,
            Uri = new Uri("/users", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { MediaType.JSON },
            Content = Encoding.UTF8.GetBytes("{\"Name\": \"Tom\", \"Age\": 33}"),
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_BasicPUT_MethodNotAllowed()
        {
            var m = new RestWebServer();
            m.RegisterController<RaiyDayTestController>();
            var response = await m.HandleRequest(_methodNotAllowedPUT);

            string content = response.ToString();

            StringAssert.Contains(content, "405 Method Not Allowed");
            StringAssert.Contains(content, "Allow: POST");
        }
        #endregion

        #region ParameterParseException
        private HttpServerRequest _parameterParseExceptionPUT = new HttpServerRequest()
        {
            Method = HttpMethod.PUT,
            Uri = new Uri("/users/notanumber", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { MediaType.JSON },
            Content = Encoding.UTF8.GetBytes("{\"Name\": \"Tom\", \"Age\": 33}"),
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_WrongParameterTypePUT_BadRequest()
        {
            var m = new RestWebServer();
            m.RegisterController<RaiyDayTestController>();
            var response = await m.HandleRequest(_parameterParseExceptionPUT);

            StringAssert.Contains(response.ToString(), "400 Bad Request");
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

        #region JsonContentParameterValueParseException
        private HttpServerRequest _contentParameterParseExPOST = new HttpServerRequest()
        {
            Method = HttpMethod.POST,
            Uri = new Uri("/users", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { MediaType.JSON },
            Content = Encoding.UTF8.GetBytes("{\"Name\": \"Tom\", \"Age\": notanumber}"),
            IsComplete = true
        };


        [TestMethod]
        public async Task HandleRequest_InvalidJSONContentParameter_BadRequest()
        {
            var m = new RestWebServer();
            m.RegisterController<RaiyDayTestController>();
            var response = await m.HandleRequest(_contentParameterParseExPOST);

            StringAssert.Contains(response.ToString(), "400 Bad Request");
        }
        #endregion

        #region XmlContentParameterValueParseException
        private HttpServerRequest _xmlContentParameterParseExPOST = new HttpServerRequest()
        {
            Method = HttpMethod.POST,
            Uri = new Uri("/users", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { MediaType.JSON },
            Content = Encoding.UTF8.GetBytes("<User><Name>Tom</Name><Age>thirtythree</Age></User>"),
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_InvalidXMLContentParameter_BadRequest()
        {
            var m = new RestWebServer();
            m.RegisterController<RaiyDayTestController>();
            var response = await m.HandleRequest(_xmlContentParameterParseExPOST);

            StringAssert.Contains(response.ToString(), "400 Bad Request");
        }
        #endregion

        #region InvalidJsonFormatParseException
        private HttpServerRequest _invalidJsonFormatPOST = new HttpServerRequest()
        {
            Method = HttpMethod.POST,
            Uri = new Uri("/users", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { MediaType.JSON },
            Content = Encoding.UTF8.GetBytes("{\"Name\": \"Tom\"; \"Age\": 33}"),
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_InvalidJsonFormat_BadRequest()
        {
            var m = new RestWebServer();
            m.RegisterController<RaiyDayTestController>();
            var response = await m.HandleRequest(_invalidJsonFormatPOST);

            StringAssert.Contains(response.ToString(), "400 Bad Request");
        }
        #endregion

        #region InvalidXmlFormatParseException
        private HttpServerRequest _invalidXmlFormatExPOST = new HttpServerRequest()
        {
            Method = HttpMethod.POST,
            Uri = new Uri("/users", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { MediaType.JSON },
            Content = Encoding.UTF8.GetBytes("<User><Name>Tom</><Age>thirtythree</Age></User>"),
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_InvalidJsonContentParameter_BadRequest()
        {
            var m = new RestWebServer();
            m.RegisterController<RaiyDayTestController>();
            var response = await m.HandleRequest(_invalidXmlFormatExPOST);

            StringAssert.Contains(response.ToString(), "400 Bad Request");
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
        public PostResponse CreateUser([FromContent] User user)
        {
            return new PostResponse(PostResponse.ResponseStatus.Conflict);
        }

        [UriFormat("/users/{id}")]
        public PutResponse UpdateUser(int id, [FromContent] User user)
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
