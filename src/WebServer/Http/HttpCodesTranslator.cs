using Devkoes.Restup.WebServer.Models.Schemas;
using System;
using System.Collections.Generic;

namespace Devkoes.Restup.WebServer.Http
{
    internal class HttpCodesTranslator
    {
        internal static HttpCodesTranslator Default { get; }

        private readonly IDictionary<MediaType, string> _mediaTypeTexts = new Dictionary<MediaType, string>()
        {
            [MediaType.JSON] = "application/json",
            [MediaType.XML] = "application/xml"
        };

        private readonly IDictionary<string, MediaType> _textAsMediaType = new Dictionary<string, MediaType>()
        {
            ["application/json"] = MediaType.JSON,
            ["text/json"] = MediaType.JSON,
            ["application/xml"] = MediaType.XML,
            ["text/xml"] = MediaType.XML,
        };

        private readonly IDictionary<int, string> _statusCodeTexts = new Dictionary<int, string>()
        {
            [200] = "OK",
            [201] = "Created",
            [204] = "No Content",
            [400] = "Bad Request",
            [404] = "Not Found",
            [405] = "Method Not Allowed",
            [409] = "Conflict",
        };

        static HttpCodesTranslator()
        {
            Default = new HttpCodesTranslator();
        }

        private HttpCodesTranslator() { }

        internal HttpMethod GetVerb(string verb)
        {
            foreach (var name in Enum.GetNames(typeof(HttpMethod)))
            {
                if (string.Equals(verb, name, StringComparison.OrdinalIgnoreCase))
                {
                    return (HttpMethod)Enum.Parse(typeof(HttpMethod), name);
                }
            }

            return HttpMethod.Unsupported;
        }

        internal string GetHttpStatusCodeText(int statusCode)
        {
            return _statusCodeTexts[statusCode];
        }

        internal string GetMediaType(MediaType mediaType)
        {
            return _mediaTypeTexts[mediaType];
        }

        internal MediaType GetMediaType(string value)
        {
            if (_textAsMediaType.ContainsKey(value))
            {
                return _textAsMediaType[value];
            }

            return MediaType.Unsupported;
        }
    }
}
