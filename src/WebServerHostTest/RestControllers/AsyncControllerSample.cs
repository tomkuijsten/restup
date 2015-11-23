using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using System;
using System.Threading.Tasks;

namespace WebServerHostTest.RestControllers
{
    [RestController(InstanceCreationType.PerCall)]
    public class AsyncControllerSample
    {
        /// <summary>
        /// You can use the normal async/await syntax. Note that the type parameter of the returning Task'T should be
        /// one of the supported responses.
        /// </summary>
        /// <returns></returns>
        [UriFormat("/async")]
        public async Task<GetResponse> GetSomethingAsync()
        {
            return await Task.FromResult(new GetResponse(GetResponse.ResponseStatus.OK, "asyncvalue"));
        }
    }
}
