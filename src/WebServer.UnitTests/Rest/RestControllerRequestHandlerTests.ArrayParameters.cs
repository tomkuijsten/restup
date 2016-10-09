using Microsoft.VisualStudio.TestTools.UnitTesting;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using Restup.Webserver.Rest;
using Restup.Webserver.UnitTests.TestHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restup.Webserver.UnitTests.Rest
{
    [TestClass]
    public class RestControllerRequestHandlerTests_ArrayParameters
    {
        [TestMethod]
        public void HandleRequest_ComplexObjectArray_Exception()
        {
            try
            {
                var restHandler = new RestControllerRequestHandler();
                restHandler.RegisterController<CopmlexArraysTestController>();

                Assert.Fail("Expected exception for complex data types in IEnumerable");
            }
            catch (InvalidOperationException)
            {
            }
            catch
            {
                throw;
            }
        }

        [TestMethod]
        public async Task HandleRequest_CharArrayParam_ArrayAsResult()
        {
            var testData = new List<char>(new[] { 'a', 'b', 'c' });
            string uri = "/arrays/chars/" + CreateUriArrayParameter(testData);
            await TestArrays(testData, uri);
        }

        [TestMethod]
        public async Task HandleRequest_BooleanArrayParam_ArrayAsResult()
        {
            var testData = new List<bool>(new[] { true, false, true, false });
            string uri = "/arrays/bools/" + CreateUriArrayParameter(testData);
            await TestArrays(testData, uri);
        }

        [TestMethod]
        public async Task HandleRequest_DoubleArrayParam_ArrayAsResult()
        {
            var testData = new List<double>(new[] { 1.0, 2.2, 3.5, 4.6, 5.123 });
            string uri = "/arrays/doubles/" + CreateUriArrayParameter(testData);
            await TestArrays(testData, uri);
        }

        [TestMethod]
        public async Task HandleRequest_IntArrayParam_ArrayAsResult()
        {
            var testData = new List<int>(new[] { 1, 2, 3, 4, 5 });
            string uri = "/users/" + CreateUriArrayParameter(testData);
            await TestArrays(testData, uri);
        }

        [TestMethod]
        public async Task HandleRequest_IntArrayQueryParam_ArrayAsResult()
        {
            var testData = new List<int>(new[] { 1, 2, 3, 4, 5 });
            string uri = "/users?ids=" + CreateUriArrayParameter(testData);
            await TestArrays(testData, uri);
        }

        [TestMethod]
        public async Task HandleRequest_IntArrayQueryParamWithExtraParam_ArrayAsResult()
        {
            var testData = new List<int>(new[] { 1, 2, 3, 4, 5 });
            string uri = "/users?ids=" + CreateUriArrayParameter(testData) + "&filter=test";
            await TestArrays(testData, uri);
        }

        [TestMethod]
        public async Task HandleRequest_IntArrayParamWithExtraParam_ArrayAsResult()
        {
            var testData = new List<int>(new[] { 1, 2, 3, 4, 5 });
            string uri = "/users/" + CreateUriArrayParameter(testData) + "/content/23";
            await TestArrays(testData, uri);
        }

        [TestMethod]
        public async Task HandleRequest_StringArrayParam_ArrayAsResult()
        {
            var testData = new List<string>(new[] { "dit", "is", "kewl" });
            string uri = "/data/" + CreateUriArrayParameter(testData);
            await TestArrays(testData, uri);
        }

        [TestMethod]
        public async Task HandleRequest_EscapedStringArrayParam_DescapedArrayAsResult()
        {
            var orgData = new List<string>() { "dit ", "i;s", "k'0e,wl" };
            var uriData = orgData.Select(s => Uri.EscapeDataString(s));
            var testData = new List<string>(uriData);
            string uri = "/data/" + CreateUriArrayParameter(testData);
            await TestArrays(orgData, uri);
        }

        [TestMethod]
        public async Task HandleRequest_EscapedStringArrayParamWithExtraParam_DescapedArrayAsResult()
        {
            var orgData = new List<string>() { "dit ", "i;s", "k'0e,wl" };
            var uriData = orgData.Select(s => Uri.EscapeDataString(s));
            var testData = new List<string>(uriData);
            string uri = "/data/" + CreateUriArrayParameter(testData) + "/content/23";
            await TestArrays(orgData, uri);
        }

        [TestMethod]
        public async Task HandleRequest_EscapedStringQueryArrayParam_DescapedArrayAsResult()
        {
            var orgData = new List<string>() { "dit ", "i;s", "k'0e,wl" };
            var uriData = orgData.Select(s => Uri.EscapeDataString(s));
            var testData = new List<string>(uriData);
            string uri = "/data?tags=" + CreateUriArrayParameter(testData);
            await TestArrays(orgData, uri);
        }

        [TestMethod]
        public async Task HandleRequest_EscapedStringQueryArrayParamWithExtraParam_DescapedArrayAsResult()
        {
            var orgData = new List<string>() { "dit ", "i;s", "k'0e,wl" };
            var uriData = orgData.Select(s => Uri.EscapeDataString(s));
            var testData = new List<string>(uriData);
            string uri = "/data?tags=" + CreateUriArrayParameter(testData) + "&filter=test";
            await TestArrays(orgData, uri);
        }

        public string CreateUriArrayParameter<T>(List<T> array)
        {
            return string.Join(";", array);
        }

        public async Task TestArrays<T>(List<T> responseArray, string uri)
        {
            var restHandler = new RestControllerRequestHandler();
            restHandler.RegisterController<ArraysTestController>();

            var request = await restHandler.HandleRequestAsync(Utils.CreateRestServerRequest(uri: new Uri(uri, UriKind.Relative)));

            Assert.IsNotNull(request);
            Assert.AreEqual((int)GetResponse.ResponseStatus.OK, request.StatusCode);

            var asGet = request as IGetResponse;
            var asArray = asGet.ContentData as ICollection;

            CollectionAssert.AreEqual(asArray, responseArray);
        }

        [RestController(InstanceCreationType.Singleton)]
        public class CopmlexArraysTestController
        {
            [UriFormat("/arrays/complex/{complexObjects}")]
            public GetResponse UnsupportedType(IEnumerable<ArraysTestController> copmlexObjects)
            {
                return null;
            }
        }

        [RestController(InstanceCreationType.Singleton)]
        public class ArraysTestController
        {
            [UriFormat("/arrays/chars/{ids}")]
            public GetResponse GetChars(IEnumerable<char> ids)
            {
                return new GetResponse(GetResponse.ResponseStatus.OK, ids);
            }

            [UriFormat("/arrays/bools/{ids}")]
            public GetResponse GetBooleans(IEnumerable<bool> ids)
            {
                return new GetResponse(GetResponse.ResponseStatus.OK, ids);
            }

            [UriFormat("/arrays/doubles/{ids}")]
            public GetResponse GetDoubles(IEnumerable<double> ids)
            {
                return new GetResponse(GetResponse.ResponseStatus.OK, ids);
            }

            [UriFormat("/users/{userIds}")]
            public GetResponse GetUsers(IEnumerable<int> userIds)
            {
                return new GetResponse(GetResponse.ResponseStatus.OK, userIds);
            }

            [UriFormat("/users/{userIds}/content/{contentId}")]
            public GetResponse GetUsersWithDetail(IEnumerable<int> userIds, int contentId)
            {
                return new GetResponse(GetResponse.ResponseStatus.OK, userIds);
            }

            [UriFormat("/users?ids={userIds}")]
            public GetResponse GetUsersAsQueryParam(IEnumerable<int> userIds)
            {
                return new GetResponse(GetResponse.ResponseStatus.OK, userIds);
            }

            //          /users?ids=1;2;3;4;5&filter=test
            [UriFormat("/users?ids={userIds}&filter={filter}")]
            public GetResponse GetUsersAsQueryWithMultipleParam(IEnumerable<int> userIds, string filter)
            {
                return new GetResponse(GetResponse.ResponseStatus.OK, userIds);
            }

            [UriFormat("/data/{tags}")]
            public GetResponse GetTags(IEnumerable<string> tags)
            {
                return new GetResponse(GetResponse.ResponseStatus.OK, tags);
            }

            [UriFormat("/data/{tags}/content/{contentId}")]
            public GetResponse GetTagsWithDetail(IEnumerable<string> tags, int contentId)
            {
                return new GetResponse(GetResponse.ResponseStatus.OK, tags);
            }

            [UriFormat("/data?tags={tags}")]
            public GetResponse GetTagsAsQueryParam(IEnumerable<string> tags)
            {
                return new GetResponse(GetResponse.ResponseStatus.OK, tags);
            }

            [UriFormat("/data?tags={tags}&filter={filter}")]
            public GetResponse GetTagsAsQueryWithMultipleParam(IEnumerable<string> tags, string filter)
            {
                return new GetResponse(GetResponse.ResponseStatus.OK, tags);
            }
        }
    }
}
