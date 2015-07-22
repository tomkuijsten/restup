using Devkoes.Restup.WebServer.Models.Schemas;
using System;

namespace Devkoes.Restup.WebServer.Helpers
{
    internal class HttpHelpers
    {
        internal static RestVerb GetVerb(string verb)
        {
            foreach(var name in Enum.GetNames(typeof(RestVerb)))
            {
                if(string.Equals(verb, name, StringComparison.OrdinalIgnoreCase))
                {
                    return (RestVerb)Enum.Parse(typeof(RestVerb), name);
                }
            }

            throw new NotSupportedException($"Unsupported verb '{verb}' found.");
        }

        internal static bool IsSupportedVerb(string verb)
        {
            foreach (var name in Enum.GetNames(typeof(RestVerb)))
            {
                if (string.Equals(verb, name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
