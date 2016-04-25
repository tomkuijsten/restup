using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Restup.HttpMessage.Models.Schemas;
using Windows.ApplicationModel;

namespace Restup.Webserver.UnitTests.File
{
    [TestClass]
    public class StaticFileRouteHandlerTests
    {
        [TestMethod]
        public void RequestOnRootIsRoutedToIndexHtml()
        {
            new StaticFileRouteHandlerFluentTests()
                .Given
                    .SetUp(basePath: null)
                    .FileExists(Package.Current.InstalledLocation.Path + @"\index.html", "test test test")
                .When
                    .GetRequestReceived("/")
                .Then
                    .AssertOkResponseExists()
                    .AssertResponse(x => x.ContentType, "text/html")
                    .AssertResponseContent("test test test");
        }

        [TestMethod]
        public void ContentTypeFromFileIsTakenIntoAccount()
        {
            new StaticFileRouteHandlerFluentTests()
                .Given
                    .SetUp(basePath: null)
                    .FileExists(Package.Current.InstalledLocation.Path + @"\path.js", extension: ".js", contentType: "application/x-javascript")
                .When
                    .GetRequestReceived("/path.js")
                .Then
                    .AssertOkResponseExists()
                    .AssertResponse(x => x.ContentType, "application/x-javascript");
        }

        [TestMethod]
        public void WhenFileDoesNotHaveAContentType()
        {
            new StaticFileRouteHandlerFluentTests()
                .Given
                    .SetUp(basePath: null)
                    .FileExists(Package.Current.InstalledLocation.Path + @"\path.js", extension: ".js", contentType: null)
                .When
                    .GetRequestReceived("/path.js")
                .Then
                    .AssertOkResponseExists()
                    .AssertResponse(x => x.ContentType, "application/x-javascript");
        }

        [TestMethod]
        public void RelativeBasePathIsUsedInFileResolution()
        {
            new StaticFileRouteHandlerFluentTests()
                .Given
                    .SetUp(basePath: @"\wwwroot\")
                    .FileExists(Package.Current.InstalledLocation.Path + @"\wwwroot\path.html")
                .When
                    .GetRequestReceived("/path.html")
                .Then
                    .AssertOkResponseExists();
        }

        [TestMethod]
        public void AbsoluteBasePathIsUsedInFileResolution()
        {
            new StaticFileRouteHandlerFluentTests()
                .Given
                    .SetUp(basePath: @"c:\wwwroot\")
                    .FileExists(@"c:\wwwroot\path.html")
                .When
                    .GetRequestReceived("/path.html")
                .Then
                    .AssertOkResponseExists();
        }

        [TestMethod]
        public void WhenFileDoesNotExistANotFoundRequestIsReturned()
        {
            new StaticFileRouteHandlerFluentTests()
                .Given
                    .SetUp(basePath: @"c:\wwwroot\")
                    .FileExists(@"c:\wwwroot\path.html")
                .When
                    .GetRequestReceived("/pathbla.html")
                .Then
                    .AssertFileNotFoundResponseExists();
        }

        [TestMethod]
        public void IgnoreQueryPart()
        {
            new StaticFileRouteHandlerFluentTests()
                .Given
                    .SetUp(basePath: @"c:\wwwroot\")
                    .FileExists(@"c:\wwwroot\path.html")
                .When
                    .GetRequestReceived("/path.html?key=value")
                .Then
                    .AssertOkResponseExists();
        }

        [TestMethod]
        [DataRow(HttpMethod.POST)]
        [DataRow(HttpMethod.DELETE)]
        [DataRow(HttpMethod.PUT)]
        [DataRow(HttpMethod.Unsupported)]
        [DataRow(null)]
        public void UnsupportedHttpMethodReturnsMethodNotAllowed(HttpMethod httpMethod)
        {
            new StaticFileRouteHandlerFluentTests()
                .Given
                    .SetUp(basePath: null)
                    .FileExists(Package.Current.InstalledLocation.Path + @"\index.html", "test test test")
                .When
                    .GetRequestReceived("/index.html", httpMethod)
                .Then
                    .AssertMethodNotAllowedResponseExists();
        }

        [TestMethod]
        public void GetHttpMethodReturnsOkRequest()
        {
            new StaticFileRouteHandlerFluentTests()
                .Given
                    .SetUp(basePath: null)
                    .FileExists(Package.Current.InstalledLocation.Path + @"\index.html", "test test test")
                .When
                    .GetRequestReceived("/index.html", HttpMethod.GET)
                .Then
                    .AssertOkResponseExists();
        }
    }
}
