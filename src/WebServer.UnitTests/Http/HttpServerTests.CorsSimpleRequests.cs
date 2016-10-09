using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        [TestMethod]
        public void SimpleRequest_WithOnlyTheExactOriginEnabled_ThenResponseShouldHaveTheAccessControlAllowOriginHeader()
        {
            new FluentHttpServerTests()
                .Given
                    .ListeningOnDefaultRoute()
                    .CorsIsEnabled(x => x.AddAllowedOrigin("http://testrequest.com"))
                .When
                    .RequestHasArrived("/Get", origin: "http://testrequest.com")
                .Then
                    .AssertRouteHandlerReceivedRequest()
                    .AssertLastResponse<AccessControlAllowOriginHeader, string>(x => x.Value, "http://testrequest.com");
        }

        [TestMethod]
        public void SimpleRequest_WithMultipleExactOriginsEnabled_ThenResponseShouldHaveTheAccessControlAllowOriginHeader()
        {
            new FluentHttpServerTests()
                .Given
                    .ListeningOnDefaultRoute()
                    .CorsIsEnabled(x => x
                        .AddAllowedOrigin("http://testrequest.com")
                        .AddAllowedOrigin("http://testrequest2.com"))
                .When
                    .RequestHasArrived("/Get", origin: "http://testrequest2.com")
                .Then
                    .AssertRouteHandlerReceivedRequest()
                    .AssertLastResponse<AccessControlAllowOriginHeader, string>(x => x.Value, "http://testrequest2.com");
        }
    }
}