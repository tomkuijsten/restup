using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Schemas;
using Restup.Webserver.Rest;
using Restup.Webserver.UnitTests.TestHelpers;
using System;
using System.Collections.Generic;

namespace Restup.Webserver.UnitTests.Rest
{
    [TestClass]
    public class RestControllerRequestHandlerArrayParameters
    {
        [TestMethod]
        public void GetRestMethods_HasIAsyncOperationMethod_CanHandleRequest()
        {
            var restHandler = new RestControllerRequestHandler();
            restHandler.RegisterController<ArraysTestController>();

            var request = restHandler.HandleRequest(Utils.CreateRestServerRequest(uri: new Uri("/users/1;2;3;4;5", UriKind.Relative)));

            Assert.IsNotNull(request.Result);
            Assert.AreEqual((int)GetResponse.ResponseStatus.OK, request.Result.StatusCode);
        }

        [RestController(InstanceCreationType.Singleton)]
        public class ArraysTestController
        {
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

            [UriFormat("/users?ids={userIds}&filter={filter}")]
            public GetResponse GetUsersAsQueryWithMultipleParam(IEnumerable<int> userIds, string filter)
            {
                return new GetResponse(GetResponse.ResponseStatus.OK, userIds);
            }

            [UriFormat("/users/{tags}")]
            public GetResponse GetTags(IEnumerable<string> tags)
            {
                return new GetResponse(GetResponse.ResponseStatus.OK, tags);
            }

            [UriFormat("/users/{tags}/content/{contentId}")]
            public GetResponse GetTagsWithDetail(IEnumerable<string> tags, int contentId)
            {
                return new GetResponse(GetResponse.ResponseStatus.OK, tags);
            }

            [UriFormat("/users?tags={tags}")]
            public GetResponse GetTagsAsQueryParam(IEnumerable<string> tags)
            {
                return new GetResponse(GetResponse.ResponseStatus.OK, tags);
            }

            [UriFormat("/users?tags={tags}&filter={filter}")]
            public GetResponse GetTagsAsQueryWithMultipleParam(IEnumerable<string> tags, string filter)
            {
                return new GetResponse(GetResponse.ResponseStatus.OK, tags);
            }
        }
    }
}
