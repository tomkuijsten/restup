using Devkoes.HttpMessage.Models.Schemas;

namespace Devkoes.HttpMessage.Plumbing
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
