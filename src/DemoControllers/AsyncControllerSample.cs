using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Restup.DemoControllers
{
    [RestController(InstanceCreationType.PerCall)]
    public sealed class AsyncControllerSample
    {
        /// <summary>
        /// You can use the normal async/await syntax. Note that the type parameter of the returning Task'T should be
        /// one of the supported responses.
        /// </summary>
        /// <returns></returns>
        [UriFormat("/async")]
        public IAsyncOperation<IGetResponse> GetSomethingAsync()
        {
            return Task.FromResult<IGetResponse>(new GetResponse(GetResponse.ResponseStatus.OK, "asyncvalue")).AsAsyncOperation();
        }
    }
}
