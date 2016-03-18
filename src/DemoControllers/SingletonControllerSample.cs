using Devkoes.Restup.DemoControllers.Model;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Rest.Models.Contracts;

namespace Devkoes.Restup.DemoControllers
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
    public IGetResponse GetSingletonSampleValueWithParameter(string v) {
      long.TryParse(v, out _totalNrOfCallsHandled);
      return new GetResponse(
          GetResponse.ResponseStatus.OK,
          new WebserverInfo() { TotalCallsHandled = _totalNrOfCallsHandled++ });
    }
  }
}
