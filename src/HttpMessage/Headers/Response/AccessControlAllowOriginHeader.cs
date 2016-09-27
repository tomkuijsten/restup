using Restup.HttpMessage.Models.Schemas;

namespace Restup.HttpMessage.Headers.Response
{
    public class AccessControlAllowOriginHeader : HttpHeaderBase
    {
        internal static string NAME = "Access-Control-Allow-Origin";

        public AccessControlAllowOriginHeader(string value) : base(NAME, value)
        {
        }
    }
}