using System;
using Restup.HttpMessage.Models.Schemas;

namespace Restup.HttpMessage.ServerRequestParsers
{
    internal class HttpMethodParser
    {
        public static HttpMethod GetMethod(string method)
        {
            method = method.ToUpper();

            HttpMethod methodVerb = HttpMethod.Unsupported;

            Enum.TryParse<HttpMethod>(method, out methodVerb);

            return methodVerb;
        }
    }
}