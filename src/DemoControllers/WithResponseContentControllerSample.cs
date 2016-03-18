using Devkoes.Restup.DemoControllers.Model;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Rest.Models.Contracts;

namespace Devkoes.Restup.DemoControllers
{
    /// <summary>
    /// Sample controller with all verbs supporting a response content.
    /// </summary>
    [RestController(InstanceCreationType.PerCall)]
    public sealed class WithResponseContentControllerSample
    {
        [UriFormat("/withresponsecontent")]
        public IPutResponse UpdateSomething()
        {
            return new PutResponse(
                PutResponse.ResponseStatus.OK,
                new ResponseData() { Status = "PUT received" });
        }

        [UriFormat("/withresponsecontent")]
        public IPostResponse CreateSomething()
        {
            return new PostResponse(
                PostResponse.ResponseStatus.Created,
                "newlocation",
                new ResponseData() { Status = "POST received" });
        }

        [UriFormat("/withresponsecontent")]
        public IGetResponse GetSomething()
        {
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                new ResponseData() { Status = "GET received" });
        }
    }
}
