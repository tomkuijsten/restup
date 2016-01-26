using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using System;

namespace Devkoes.Restup.DemoControllers
{
    [RestController(InstanceCreationType.PerCall)]
    public class PerCallControllerSample
    {
        private long _totalNrOfCallsHandled;

        private class CallInfo
        {
            public long TotalCallsHandled { get; set; }
        }

        /// <summary>
        /// This will always return a TotalCallsHandled of one, no matter how many times you request this uri.
        /// </summary>
        /// <returns></returns>
        [UriFormat("/percall")]
        public GetResponse GetPerCallSampleValue()
        {
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                new CallInfo() { TotalCallsHandled = _totalNrOfCallsHandled++ });
        }
    }
}
