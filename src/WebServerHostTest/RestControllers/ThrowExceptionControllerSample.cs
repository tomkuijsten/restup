using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using System;

namespace Devkoes.Restup.WebServerHostTest.RestControllers
{
    [RestController(InstanceCreationType.Singleton)]
    public class ThrowExceptionControllerSample
    {
        /// <summary>
        /// The webserver will catch this exception, and return a BadRequest HTTP result.
        /// </summary>
        /// <returns></returns>
        [UriFormat("/throwexception")]
        public GetResponse GetSingletonSampleValue()
        {
            throw new NotImplementedException("Sorry, didn't implement this functionality yet.");
        }
    }
}
