using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using System;
using System.Diagnostics;

namespace Devkoes.Restup.WebServerHostTest.RestControllers
{
    [RestController(InstanceCreationType.PerCall)]
    public class FromContentControllerSample
    {
        public struct FromContentData
        {
            public int Counter { get; set; }
        }

        [UriFormat("/fromcontent")]
        public PutResponse UpdateSomething([FromContent] FromContentData data)
        {
            Debug.WriteLine($"Received counter value of {data.Counter}");
            return new PutResponse(PutResponse.ResponseStatus.OK);
        }

        [UriFormat("/fromcontent")]
        public PostResponse CreateSomething([FromContent] FromContentData data)
        {
            Debug.WriteLine($"Received counter value of {data.Counter}");
            return new PostResponse(PostResponse.ResponseStatus.Created, $"fromcontent/{data.Counter}");
        }
    }
}
