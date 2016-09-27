using System.Collections.Generic;

namespace Restup.HttpMessage.Headers.Response
{
    public class AccessControlAllowHeadersHeader : HttpHeaderBase
    {
        internal static string NAME = "Access-Control-Allow-Headers";

        public AccessControlAllowHeadersHeader(IEnumerable<string> headers) : base(NAME, string.Join(", ", headers))
        {
        }
    }
}