using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Schemas;
using System;

namespace Restup.Webserver.UnitTests.TestHelpers
{

    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    [RestController(InstanceCreationType.Singleton)]
    public class HappyPathTestController
    {
        [UriFormat("/users/{userId}")]
        public GetResponse GetUser(int userId)
        {
            return new GetResponse(GetResponse.ResponseStatus.OK, new User() { Name = "Tom", Age = 30 });
        }

        [UriFormat("/users?userId={userId}")]
        public GetResponse GetUserWithParam(int userId)
        {
            return new GetResponse(GetResponse.ResponseStatus.OK, new User() { Name = "Tom", Age = 30 });
        }

        [UriFormat("/users")]
        public PostResponse CreateUser([FromContent] User user)
        {
            return new PostResponse(PostResponse.ResponseStatus.Created, $"/users/2");
        }

        [UriFormat("/users/{userId}")]
        public PutResponse UpdateUser(int userId)
        {
            return new PutResponse(PutResponse.ResponseStatus.OK);
        }

        [UriFormat("/users/{userId}")]
        public DeleteResponse DeleteUser(int userId)
        {
            return new DeleteResponse(DeleteResponse.ResponseStatus.OK);
        }
    }

    [RestController(InstanceCreationType.Singleton)]
    public class HappyPathTestSingletonControllerWithArgs
    {
        private User _user;

        public HappyPathTestSingletonControllerWithArgs(string name, int age)
        {
            _user = new User() { Name = name, Age = age };
        }

        [UriFormat("/users/{userId}")]
        public GetResponse GetUser(int userId)
        {
            return new GetResponse(GetResponse.ResponseStatus.OK, _user);
        }
    }

    [RestController(InstanceCreationType.PerCall)]
    public class HappyPathTestPerCallControllerWithArgs
    {
        private User _user;

        public HappyPathTestPerCallControllerWithArgs(string name, int age)
        {
            _user = new User() { Name = name, Age = age };
        }

        [UriFormat("/users/{userId}")]
        public GetResponse GetUser(int userId)
        {
            return new GetResponse(GetResponse.ResponseStatus.OK, _user);
        }
    }
}
