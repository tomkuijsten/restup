namespace Restup.HttpMessage.Headers.Response
{
    public class AccessControlAllowCredentialsHeader : HttpHeaderBase
    {
        internal static string NAME = "Access-Control-Allow-Credentials";

        public AccessControlAllowCredentialsHeader(bool value) : base(NAME, ConvertToString(value))
        {
        }

        private static string ConvertToString(bool value)
        {
            return value.ToString().ToLowerInvariant();
        }
    }
}