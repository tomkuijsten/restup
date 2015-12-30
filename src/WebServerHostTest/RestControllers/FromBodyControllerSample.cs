using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using System;
using System.Diagnostics;

namespace Devkoes.Restup.WebServerHostTest.RestControllers
{
    [RestController(InstanceCreationType.PerCall)]
    public class FromBodyControllerSample
    {
        public struct FromBodyData
        {
            public int Counter { get; set; }
        }

        [UriFormat("/frombody")]
        public PutResponse UpdateSomething([FromBody] FromBodyData data)
        {
            Debug.WriteLine($"Received counter value of {data.Counter}");
            return new PutResponse(PutResponse.ResponseStatus.OK);
        }

        [UriFormat("/frombody")]
        public PostResponse CreateSomething([FromBody] FromBodyData data)
        {
            Debug.WriteLine($"Received counter value of {data.Counter}");
            return new PostResponse(PostResponse.ResponseStatus.Created, $"frombody/{data.Counter}");
        }
    }
}
