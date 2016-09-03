using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Restup.HttpMessage.Headers.Response;
using Restup.Webserver.UnitTests.TestHelpers;

namespace Restup.Webserver.UnitTests.Http
{
    [TestClass]
    public class HttpServerTests_CorsSimpleRequests
    {
        [TestMethod]
        public void SimpleRequest_WithOriginHeaderSpecified_ThenResponseShouldHaveTheAccessControlAllowOriginHeader()
        {
            new FluentHttpServerTests()
                .Given
                    .ListeningOnDefaultRoute()
                    .CorsIsEnabled()
                .When
                    .RequestHasArrived("/Get", origin: "http://testrequest.com")
                .Then
                    .AssertRouteHandlerReceivedRequest()
                    .AssertLastResponse<AccessControlAllowOriginHeader, string>(x => x.Value, "*");
        }

        [TestMethod]
        public void SimpleRequest_WithNoOriginHeaderSpecified_ThenResponseShouldNotHaveTheAccessControlAllowOriginHeader()
        {
            // todo: edge case: the origin header can be null but present, how to deal with that?
            new FluentHttpServerTests()
                .Given
                    .ListeningOnDefaultRoute()
                .When
                    .RequestHasArrived("/Get")
                .Then
                    .AssertRouteHandlerReceivedRequest()
                    .AssertLastResponseHasNoHeaderOf<AccessControlAllowOriginHeader>();
        }
    }
}