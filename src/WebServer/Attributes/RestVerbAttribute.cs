using System;

namespace Devkoes.Restup.WebServer.Attributes
{
    [System.AttributeUsage((AttributeTargets.Class | AttributeTargets.Struct), Inherited = false, AllowMultiple = false)]
    public sealed class RestVerbAttribute : Attribute
    {
        public RestVerb Verb { get; }

        public RestVerbAttribute(RestVerb verb)
        {
            Verb = verb;
        }
    }
}
