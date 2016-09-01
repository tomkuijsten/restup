using System;

namespace Restup.HttpMessage.Headers.Response
{
    public class AccessControlMaxAgeHeader : HttpHeaderBase
    {
        internal static string NAME = "Access-Control-Max-Age";

        public AccessControlMaxAgeHeader(int deltaInSeconds) : base(NAME, Convert.ToString(deltaInSeconds))
        {
        }
    }
}