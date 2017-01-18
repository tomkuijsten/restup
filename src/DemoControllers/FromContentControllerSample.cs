using Restup.DemoControllers.Model;
using Restup.Webserver;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using Restup.WebServer.Logging;

namespace Restup.DemoControllers
{
    [RestController(InstanceCreationType.PerCall)]
    public sealed class FromContentControllerSample
    {
        private readonly ILogger _log;

        public FromContentControllerSample()
        {
            _log = LogManager.GetLogger<FromContentControllerSample>();
        }

        [UriFormat("/fromcontent")]
        public IPutResponse UpdateSomething([FromContent] FromContentData data)
        {
            _log.Debug($"Received counter value of {data.Counter}");
            return new PutResponse(PutResponse.ResponseStatus.OK);
        }

        [UriFormat("/fromcontent")]
        public IPostResponse CreateSomething([FromContent] FromContentData data)
        {
            _log.Debug($"Received counter value of {data.Counter}");
            return new PostResponse(PostResponse.ResponseStatus.Created, $"fromcontent/{data.Counter}");
        }

        [UriFormat("/stringencoding")]
        public IGetResponse EncodingDecodingTest([FromContent] MoreComplexData data)
        {
            _log.Debug($"Received complex data with text {data.Text}");
            return new GetResponse(GetResponse.ResponseStatus.OK, new MoreComplexData() { Text = data.Text });
        }
    }
}
