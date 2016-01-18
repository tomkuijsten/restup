using Devkoes.HttpMessage.Models.Schemas;
using System;

namespace Devkoes.Restup.WebServer.Attributes
{
    [System.AttributeUsage((AttributeTargets.Class | AttributeTargets.Struct), Inherited = false, AllowMultiple = false)]
    internal sealed class RestVerbAttribute : Attribute
    {
        public HttpMethod Verb { get; }

        public RestVerbAttribute(HttpMethod verb)
        {
            Verb = verb;
        }
    }
}
