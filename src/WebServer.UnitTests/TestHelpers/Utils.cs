using Restup.HttpMessage;
using Restup.HttpMessage.Models.Contracts;
using Restup.HttpMessage.Models.Schemas;
using Restup.Webserver.Models.Schemas;
using Restup.Webserver.Rest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restup.Webserver.UnitTests.TestHelpers
{
    // helper methods to make it easier to change the creation of classes in the future
    public static class Utils
    {
        public static RestRouteHandler CreateRestRoutehandler<T>() where T : class
        {
            var restRouteHandler = CreateRestRoutehandler();
            restRouteHandler.RegisterController<T>();
            return restRouteHandler;
        }

        public static RestRouteHandler CreateRestRoutehandler<T>(params object[] args) where T : class
        {
            var restRouteHandler = CreateRestRoutehandler();
            restRouteHandler.RegisterController<T>(args);
            return restRouteHandler;
        }

        public static RestRouteHandler CreateRestRoutehandler<T>(Func<object[]> args) where T : class
        {
            var restRouteHandler = CreateRestRoutehandler();
            restRouteHandler.RegisterController<T>(args);
            return restRouteHandler;
        }

        public static RestRouteHandler CreateRestRoutehandler()
        {
            var restRouteHandler = new RestRouteHandler();
            return restRouteHandler;
        }

        public static HttpServerResponse CreateOkHttpServerResponse(byte[] content = null)
        {
            var response = HttpServerResponse.Create(new Version(1, 1), HttpResponseStatus.OK);
            response.Content = content;

            return response;
        }

        public static IHttpServerRequest CreateHttpRequest(IEnumerable<IHttpRequestHeader> headers = null,
            HttpMethod? method = HttpMethod.GET, Uri uri = null, string httpVersion = "HTTP / 1.1",
            string contentTypeCharset = null, IEnumerable<string> acceptCharsets = null,
            int contentLength = 0, string contentType = null,
            IEnumerable<string> acceptEncodings = null,
            IEnumerable<string> acceptMediaTypes = null, byte[] content = null,
            bool isComplete = true)
        {
            return new HttpServerRequest(headers ?? Enumerable.Empty<IHttpRequestHeader>(), method,
                uri ?? new Uri("/Get", UriKind.Relative), httpVersion, contentTypeCharset,
                acceptCharsets ?? Enumerable.Empty<string>(),
                contentLength, contentType, acceptEncodings ?? Enumerable.Empty<string>(),
                acceptMediaTypes ?? Enumerable.Empty<string>(), content ?? new byte[] { },
                isComplete);
        }

        internal static RestServerRequest CreateRestServerRequest(IEnumerable<IHttpRequestHeader> headers = null,
            HttpMethod? method = HttpMethod.GET, Uri uri = null, string httpVersion = "HTTP / 1.1",
            string contentTypeCharset = null, IEnumerable<string> acceptCharsets = null,
            int contentLength = 0, string contentType = null,
            IEnumerable<string> acceptEncodings = null, IEnumerable<string> acceptMediaTypes = null,
            byte[] content = null, bool isComplete = true)
        {
            var restServerRequestFactory = new RestServerRequestFactory();
            var httpRequest = Utils.CreateHttpRequest(headers, method, uri, httpVersion, contentTypeCharset,
                acceptCharsets, contentLength, contentType, acceptEncodings, acceptMediaTypes, content, isComplete);

            return restServerRequestFactory.Create(httpRequest);
        }
    }
}