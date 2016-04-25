using Restup.DemoControllers.Model;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;

namespace Restup.DemoControllers
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
