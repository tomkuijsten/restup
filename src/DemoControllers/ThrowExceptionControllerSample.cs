using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using System;
using Devkoes.Restup.WebServer.Rest.Models.Contracts;

namespace Devkoes.Restup.DemoControllers
{
    [RestController(InstanceCreationType.Singleton)]
    public sealed class ThrowExceptionControllerSample
    {
        /// <summary>
        /// The webserver will catch this exception, and return a BadRequest HTTP result.
        /// </summary>
        /// <returns></returns>
        [UriFormat("/throwexception")]
        public IGetResponse GetSingletonSampleValue()
        {
            throw new NotImplementedException("Sorry, didn't implement this functionality yet.");
        }
    }
}
