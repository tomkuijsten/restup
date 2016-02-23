using Devkoes.HttpMessage.Models.Schemas;
using System.Collections.Generic;

namespace Devkoes.HttpMessage.Plumbing
{
    public class HttpCodesTranslator
    {
        public static HttpCodesTranslator Default { get; }

        private readonly IDictionary<MediaType, string> _mediaTypeTexts = new Dictionary<MediaType, string>()
        {
            [MediaType.Unsupported] = "",
            [MediaType.JSON] = "application/json",
            [MediaType.XML] = "application/xml",
            [MediaType.HTML] = "text/html",
            [MediaType.CSS] = "text/css",
            [MediaType.IMAGE] = "image"
        };

        private readonly IDictionary<string, MediaType> _textAsMediaType = new Dictionary<string, MediaType>()
        {
            ["application/json"] = MediaType.JSON,
            ["text/json"] = MediaType.JSON,
            ["application/xml"] = MediaType.XML,
            ["text/xml"] = MediaType.XML,
            ["text/html"] = MediaType.HTML,
            ["image"] = MediaType.IMAGE,
            ["text/css"] = MediaType.CSS,
            ["text/plain"] = MediaType.HTML
        };

        // From http://www.w3.org/Protocols/rfc2616/rfc2616-sec6.html
        private readonly IDictionary<int, string> _statusCodeTexts = new Dictionary<int, string>()
        {
            [100] = "Continue",
            [101] = "Switching Protocols",
            [200] = "OK",
            [201] = "Created",
            [202] = "Accepted",
            [203] = "Non-Authoritative Information",
            [204] = "No Content",
            [205] = "Reset Content",
            [206] = "Partial Content",
            [300] = "Multiple Choices",
            [301] = "Moved Permanently",
            [302] = "Found",
            [303] = "See Other",
            [304] = "Not Modified",
            [305] = "Use Proxy",
            [307] = "Temporary Redirect",
            [400] = "Bad Request",
            [401] = "Unauthorized",
            [402] = "Payment Required",
            [403] = "Forbidden",
            [404] = "Not Found",
            [405] = "Method Not Allowed",
            [406] = "Not Acceptable",
            [407] = "Proxy Authentication Required",
            [408] = "Request Time-out",
            [409] = "Conflict",
            [410] = "Gone",
            [411] = "Length Required",
            [412] = "Precondition Failed",
            [413] = "Request Entity Too Large",
            [414] = "Request-URI Too Large",
            [415] = "Unsupported Media Type",
            [416] = "Requested range not satisfiable",
            [417] = "Expectation Failed",
            [500] = "Internal Server Error",
            [501] = "Not Implemented",
            [502] = "Bad Gateway",
            [503] = "Service Unavailable",
            [504] = "Gateway Time-out",
            [505] = "HTTP Version not supported"
        };

        static HttpCodesTranslator()
        {
            Default = new HttpCodesTranslator();
        }

        private HttpCodesTranslator() { }

        public string GetHttpStatusCodeText(int statusCode)
        {
            return _statusCodeTexts[statusCode];
        }

        public string GetMediaType(MediaType mediaType)
        {
            return _mediaTypeTexts[mediaType];
        }

        public MediaType GetMediaType(string value)
        {
            if (_textAsMediaType.ContainsKey(value))
            {
                return _textAsMediaType[value];
            }

            return MediaType.Unsupported;
        }
    }
}
