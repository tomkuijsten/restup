using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Rest.Models.Contracts;

namespace Devkoes.Restup.DemoControllers
{
    [RestController(InstanceCreationType.PerCall)]
    public sealed class PerCallControllerSample
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
        public IGetResponse GetPerCallSampleValue()
        {
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                new CallInfo() { TotalCallsHandled = _totalNrOfCallsHandled++ });
        }
    }
}
