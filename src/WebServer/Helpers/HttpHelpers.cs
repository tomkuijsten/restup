using Devkoes.Restup.WebServer.Models.Schemas;
using System;
using System.Collections.Generic;

namespace Devkoes.Restup.WebServer.Helpers
{
    internal static class HttpHelpers
    {
        private static readonly IDictionary<MediaType, string> _mediaTypeTexts = new Dictionary<MediaType, string>()
        {
            [MediaType.JSON] = "application/json",
            [MediaType.XML] = "application/xml"
        };

        private static readonly IDictionary<int, string> _statusCodeTexts = new Dictionary<int, string>() {
            [200] = "OK",
            [201] = "Created",
            [204] = "No Content",
            [400] = "Bad Request",
            [404] = "Not Found",
            [405] = "Method Not Allowed",
            [409] = "Conflict",
        };

        internal static RestVerb GetVerb(string verb)
        {
            foreach(var name in Enum.GetNames(typeof(RestVerb)))
            {
                if(string.Equals(verb, name, StringComparison.OrdinalIgnoreCase))
                {
                    return (RestVerb)Enum.Parse(typeof(RestVerb), name);
                }
            }

            return RestVerb.Unsupported;
        }

        internal static string GetHttpStatusCodeText(int statusCode)
        {
            return _statusCodeTexts[statusCode];
        }

        internal static string GetMediaType(MediaType mediaType)
        {
            return _mediaTypeTexts[mediaType];
        }
    }
}
