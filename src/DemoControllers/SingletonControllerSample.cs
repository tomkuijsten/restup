using Restup.DemoControllers.Model;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;

namespace Restup.DemoControllers
{
    [RestController(InstanceCreationType.Singleton)]
    public sealed class SingletonControllerSample
    {
        private long _totalNrOfCallsHandled;

        [UriFormat("/singleton")]
        public IGetResponse GetSingletonSampleValue()
        {
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                new WebserverInfo() { TotalCallsHandled = _totalNrOfCallsHandled++ });
        }

        [UriFormat("/singletonwithparameter?p={v}")]
        public IGetResponse GetSingletonSampleValueWithParameter(string v)
        {
            long.TryParse(v, out _totalNrOfCallsHandled);
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                new WebserverInfo() { TotalCallsHandled = _totalNrOfCallsHandled++ });
        }
    }
}
