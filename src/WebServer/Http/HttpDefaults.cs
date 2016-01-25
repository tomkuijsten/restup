using Devkoes.HttpMessage.Models.Schemas;

namespace Devkoes.Restup.WebServer.Http
{
    internal class HttpDefaults
    {
        internal static HttpDefaults Default;

        static HttpDefaults()
        {
            Default = new HttpDefaults();
        }

        internal string GetDefaultCharset(MediaType mediaType)
        {
            if (mediaType == MediaType.JSON)
            {
                return Constants.DefaultJSONCharset;
            }

            return Constants.DefaultHttpCharset;
        }
    }
}
