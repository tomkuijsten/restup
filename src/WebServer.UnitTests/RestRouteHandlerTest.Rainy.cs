using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Restup.HttpMessage;
using Restup.HttpMessage.Models.Schemas;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Schemas;
using Restup.Webserver.UnitTests.TestHelpers;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Restup.Webserver.UnitTests
{
    [TestClass]
    public class RestRouteHandlerRainyDayTest
    {
        #region ConflictingPost
        private MutableHttpServerRequest _conflictingPOST = new MutableHttpServerRequest()
        {
            Method = HttpMethod.POST,
            Uri = new Uri("/users", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { "application/json" },
            Content = Encoding.UTF8.GetBytes("{\"Name\": \"Tom\", \"Age\": 33}"),
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_CreateWithExistingId_Conflicted()
        {
            var m = Utils.CreateRestRoutehandler<RainyDayTestController>();

            var response = await m.HandleRequest(_conflictingPOST);

            string content = response.ToString();

            StringAssert.Contains(content, "409 Conflict");
            StringAssert.DoesNotMatch(content, new Regex("Location:"));
        }
        #endregion

        #region MethodNotAllowed
        private MutableHttpServerRequest _methodNotAllowedPUT = new MutableHttpServerRequest()
        {
            Method = HttpMethod.DELETE,
            Uri = new Uri("/users", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { "text/json" },
            Content = Encoding.UTF8.GetBytes("{\"Name\": \"Tom\", \"Age\": 33}"),
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_BasicPUT_MethodNotAllowed()
        {
            var m = Utils.CreateRestRoutehandler<RainyDayTestController>();

            var response = await m.HandleRequest(_methodNotAllowedPUT);

            string content = response.ToString();

            StringAssert.Contains(content, "405 Method Not Allowed");
            StringAssert.Contains(content, "Allow: POST");
        }
        #endregion

        #region ParameterParseException
        private MutableHttpServerRequest _parameterParseExceptionPUT = new MutableHttpServerRequest()
        {
            Method = HttpMethod.PUT,
            Uri = new Uri("/users/notanumber", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { "application/json" },
            Content = Encoding.UTF8.GetBytes("{\"Name\": \"Tom\", \"Age\": 33}"),
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_WrongParameterTypePUT_BadRequest()
        {
            var m = Utils.CreateRestRoutehandler<RainyDayTestController>();

            var response = await m.HandleRequest(_parameterParseExceptionPUT);

            StringAssert.Contains(response.ToString(), "400 Bad Request");
        }
        #endregion

        #region ParameterTypeException
        [TestMethod]
        public void HandleRequest_WrongParameterTypeInController_InvalidOperationException()
        {
            var m = Utils.CreateRestRoutehandler();

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
        private MutableHttpServerRequest _contentParameterParseExPOST = new MutableHttpServerRequest()
        {
            Method = HttpMethod.POST,
            Uri = new Uri("/users", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { "text/json" },
            Content = Encoding.UTF8.GetBytes("{\"Name\": \"Tom\", \"Age\": notanumber}"),
            IsComplete = true
        };


        [TestMethod]
        public async Task HandleRequest_InvalidJSONContentParameter_BadRequest()
        {
            var m = Utils.CreateRestRoutehandler<RainyDayTestController>();

            var response = await m.HandleRequest(_contentParameterParseExPOST);

            StringAssert.Contains(response.ToString(), "400 Bad Request");
        }
        #endregion

        #region XmlContentParameterValueParseException
        private MutableHttpServerRequest _xmlContentParameterParseExPOST = new MutableHttpServerRequest()
        {
            Method = HttpMethod.POST,
            Uri = new Uri("/users", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { "application/json" },
            Content = Encoding.UTF8.GetBytes("<User><Name>Tom</Name><Age>thirtythree</Age></User>"),
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_InvalidXMLContentParameter_BadRequest()
        {
            var m = Utils.CreateRestRoutehandler<RainyDayTestController>();

            var response = await m.HandleRequest(_xmlContentParameterParseExPOST);

            StringAssert.Contains(response.ToString(), "400 Bad Request");
        }
        #endregion

        #region InvalidJsonFormatParseException
        private MutableHttpServerRequest _invalidJsonFormatPOST = new MutableHttpServerRequest()
        {
            Method = HttpMethod.POST,
            Uri = new Uri("/users", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { "application/json" },
            Content = Encoding.UTF8.GetBytes("{\"Name\": \"Tom\"; \"Age\": 33}"),
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_InvalidJsonFormat_BadRequest()
        {
            var m = Utils.CreateRestRoutehandler<RainyDayTestController>();

            var response = await m.HandleRequest(_invalidJsonFormatPOST);

            StringAssert.Contains(response.ToString(), "400 Bad Request");
        }
        #endregion

        #region InvalidXmlFormatParseException
        private MutableHttpServerRequest _invalidXmlFormatExPOST = new MutableHttpServerRequest()
        {
            Method = HttpMethod.POST,
            Uri = new Uri("/users", UriKind.RelativeOrAbsolute),
            AcceptMediaTypes = new[] { "text/json" },
            Content = Encoding.UTF8.GetBytes("<User><Name>Tom</><Age>thirtythree</Age></User>"),
            IsComplete = true
        };

        [TestMethod]
        public async Task HandleRequest_InvalidJsonContentParameter_BadRequest()
        {
            var m = Utils.CreateRestRoutehandler<RainyDayTestController>();

            var response = await m.HandleRequest(_invalidXmlFormatExPOST);

            StringAssert.Contains(response.ToString(), "400 Bad Request");
        }
        #endregion
    }

    [RestController(InstanceCreationType.Singleton)]
    public class RainyDayTestController
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
