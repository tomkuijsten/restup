using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using System;

namespace Devkoes.Restup.WebServerHostTest.RestControllers
{
    [RestController(InstanceCreationType.Singleton)]
    public class SingletonControllerSample
    {
        private long _totalNrOfCallsHandled;

        private class WebserverInfo
        {
            public long TotalCallsHandled { get; set; }
        }

        [UriFormat("/singleton")]
        public GetResponse GetSingletonSampleValue()
        {
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                new WebserverInfo() { TotalCallsHandled = _totalNrOfCallsHandled++ });
        }
    }
}
