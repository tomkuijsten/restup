using Devkoes.Restup.WebServer.Models.Schemas;
using System;

namespace Devkoes.Restup.WebServer.Attributes
{
    [System.AttributeUsage((AttributeTargets.Class | AttributeTargets.Struct), Inherited = false, AllowMultiple = false)]
    internal sealed class RestVerbAttribute : Attribute
    {
        public RestVerb Verb { get; }

        public RestVerbAttribute(RestVerb verb)
        {
            Verb = verb;
        }
    }
}
