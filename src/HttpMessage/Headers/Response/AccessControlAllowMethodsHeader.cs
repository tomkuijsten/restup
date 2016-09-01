using System.Collections.Generic;
using Restup.HttpMessage.Models.Schemas;

namespace Restup.HttpMessage.Headers.Response
{
    public class AccessControlAllowMethodsHeader : HttpHeaderBase
    {
        internal static string NAME = "Access-Control-Allow-Methods";

        public AccessControlAllowMethodsHeader(IEnumerable<HttpMethod> methods) : base(NAME, string.Join(",", methods))
        {
        }
    }
}