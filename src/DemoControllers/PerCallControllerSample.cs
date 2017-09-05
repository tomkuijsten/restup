using Restup.DemoControllers.Model;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;

namespace Restup.DemoControllers
{
    [RestController(InstanceCreationType.PerCall)]
    public sealed class PerCallControllerSample
    {
        private long _totalNrOfCallsHandled;

        /// <summary>
        /// This will always return a TotalCallsHandled of one, no matter how many times you request this uri.
        /// </summary>
        /// <returns></returns>
        [UriFormat("/percall")]
        public IGetResponse GetPerCallSampleValue()
        {
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                new WebserverInfo() { TotalCallsHandled = ++_totalNrOfCallsHandled });
        }
    }
}
