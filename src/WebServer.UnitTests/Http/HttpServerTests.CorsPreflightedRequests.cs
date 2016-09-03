using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Restup.HttpMessage.Headers.Response;
using Restup.HttpMessage.Models.Schemas;
using Restup.Webserver.UnitTests.TestHelpers;

namespace Restup.Webserver.UnitTests.Http
{
    [TestClass]
    public class HttpServerTests_CorsPreflightedRequests
    {
        [TestMethod]
        public void PreflightRequest_RequestWithoutAccessControlRequestHeaders()
        {
            new FluentHttpServerTests()
                .Given
                    .ListeningOnDefaultRoute()
                    .CorsIsEnabled()
                .When
                    .RequestHasArrived("/Get", method: HttpMethod.OPTIONS, 
                    contentType: "application/x-www-form-urlencoded",
                        origin: "http://testrequest.com",
                        accessControlRequestMethod: HttpMethod.POST)
                .Then
                    .AssertRouteHandlerReceivedNoRequests()
                    .AssertLastResponse<AccessControlAllowOriginHeader>("*")
                    .AssertLastResponse<AccessControlAllowMethodsHeader>("GET, POST, PUT, DELETE, OPTIONS")
                    .AssertLastResponseHasNoHeaderOf<AccessControlAllowHeadersHeader>()
                    .AssertLastResponse<AccessControlMaxAgeHeader>("600");
        }

        [TestMethod]
        public void PreflightRequest_WithOnlyTheExactOriginEnabled_ReturnsTrue()
        {
            new FluentHttpServerTests()
                .Given
                    .ListeningOnDefaultRoute()
                    .CorsIsEnabled("http://testrequest.com")
                .When
                    .RequestHasArrived("/Get", method: HttpMethod.OPTIONS,
                        contentType: "application/x-www-form-urlencoded",
                        origin: "http://testrequest.com",
                        accessControlRequestMethod: HttpMethod.POST)
                .Then
                    .AssertRouteHandlerReceivedNoRequests()
                    .AssertLastResponse<AccessControlAllowOriginHeader>("http://testrequest.com")
                    .AssertLastResponse<AccessControlAllowMethodsHeader>("GET, POST, PUT, DELETE, OPTIONS")
                    .AssertLastResponseHasNoHeaderOf<AccessControlAllowHeadersHeader>();
        }

        [TestMethod]
        public void PreflightRequest_WithOriginNotInTheList_ThenNoCorsHeadersArePresent()
        {
            new FluentHttpServerTests()
                .Given
                    .ListeningOnDefaultRoute()
                    .CorsIsEnabled("http://iamtheonlyoriginthatcounts.com")
                .When
                    .RequestHasArrived("/Get", method: HttpMethod.OPTIONS,
                        contentType: "application/x-www-form-urlencoded",
                        origin: "http://testrequest.com",
                        accessControlRequestMethod: HttpMethod.POST)
                .Then
                    .AssertRouteHandlerReceivedRequest()
                    .AssertLastResponseHasNoHeaderOf<AccessControlAllowOriginHeader>()
                    .AssertLastResponseHasNoHeaderOf<AccessControlAllowMethodsHeader>()
                    .AssertLastResponseHasNoHeaderOf<AccessControlAllowHeadersHeader>();
        }

        [TestMethod]
        public void PreflightRequest_RequestWithAccessControlRequestHeaders()
        {
            new FluentHttpServerTests()
                .Given
                    .ListeningOnDefaultRoute()
                    .CorsIsEnabled()
                .When
                    .RequestHasArrived( "/Get", method: HttpMethod.OPTIONS, contentType: "application/xml",
                        origin: "http://testrequest.com", 
                        accessControlRequestMethod: HttpMethod.POST,
                        accessControlRequestHeaders: new [] {"X-PINGOTHER", "application/xml"})
                .Then
                    .AssertRouteHandlerReceivedNoRequests()
                    .AssertLastResponse<AccessControlAllowOriginHeader>("*")
                    .AssertLastResponse<AccessControlAllowMethodsHeader>("GET, POST, PUT, DELETE, OPTIONS")
                    .AssertLastResponse<AccessControlAllowHeadersHeader>("X-PINGOTHER, application/xml")
                    .AssertLastResponse<AccessControlMaxAgeHeader>("600");
        }
    }
}