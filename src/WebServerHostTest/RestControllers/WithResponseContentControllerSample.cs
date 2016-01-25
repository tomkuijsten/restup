using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using System;

namespace Devkoes.Restup.WebServerHostTest.RestControllers
{
    /// <summary>
    /// Sample controller with all verbs supporting a response content.
    /// </summary>
    [RestController(InstanceCreationType.PerCall)]
    public class WithResponseContentControllerSample
    {
        public struct ResponseData
        {
            public string Status { get; set; }
        }

        [UriFormat("/withresponsecontent")]
        public PutResponse UpdateSomething()
        {
            return new PutResponse(
                PutResponse.ResponseStatus.OK,
                new ResponseData() { Status = "PUT received" });
        }

        [UriFormat("/withresponsecontent")]
        public PostResponse CreateSomething()
        {
            return new PostResponse(
                PostResponse.ResponseStatus.Created,
                "newlocation",
                new ResponseData() { Status = "POST received" });
        }

        [UriFormat("/withresponsecontent")]
        public GetResponse GetSomething()
        {
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                new ResponseData() { Status = "GET received" });
        }
    }
}
