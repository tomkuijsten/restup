using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using System;
using System.Diagnostics;
using Devkoes.Restup.DemoControllers.Model;
using Devkoes.Restup.WebServer.Rest.Models.Contracts;

namespace Devkoes.Restup.DemoControllers
{
    [RestController(InstanceCreationType.PerCall)]
    public sealed class FromContentControllerSample
    {
        [UriFormat("/fromcontent")]
        public IPutResponse UpdateSomething([FromContent] FromContentData data)
        {
            Debug.WriteLine($"Received counter value of {data.Counter}");
            return new PutResponse(PutResponse.ResponseStatus.OK);
        }

        [UriFormat("/fromcontent")]
        public IPostResponse CreateSomething([FromContent] FromContentData data)
        {
            Debug.WriteLine($"Received counter value of {data.Counter}");
            return new PostResponse(PostResponse.ResponseStatus.Created, $"fromcontent/{data.Counter}");
        }

        [UriFormat("/stringencoding")]
        public IGetResponse EncodingDecodingTest([FromContent] MoreComplexData data)
        {
            Debug.WriteLine($"Received complex data with text {data.Text}");
            return new GetResponse(GetResponse.ResponseStatus.OK, new MoreComplexData() { Text = data.Text });
        }
    }
}
