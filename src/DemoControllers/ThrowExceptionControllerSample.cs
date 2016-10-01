using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;

namespace Restup.DemoControllers
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
