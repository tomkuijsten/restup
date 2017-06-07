using Restup.HttpMessage.Models.Contracts;
using System.Text;

namespace Restup.HttpMessage.ServerResponseParsers
{
    internal class ContentParser : IHttpResponsePartParser
    {
        private static readonly Encoding DEFAULT_CONTENT_ENCODING = Encoding.UTF8;

        public byte[] ParseToBytes(HttpServerResponse response)
        {
            return response.Content ?? new byte[0];
        }

        public string ParseToString(HttpServerResponse response)
        {
            if (response.Content == null)
                return string.Empty;

            var encoding = Encoding.GetEncoding(response.ContentCharset);
            return (encoding ?? DEFAULT_CONTENT_ENCODING).GetString(response.Content);
        }
    }
}
