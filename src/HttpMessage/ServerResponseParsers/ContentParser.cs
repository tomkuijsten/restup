using Devkoes.HttpMessage.Models.Contracts;

namespace Devkoes.HttpMessage.ServerResponseParsers
{
    internal class ContentParser : IHttpResponsePartParser
    {
        public byte[] ParseToBytes(HttpServerResponse response)
        {
            var asString = ParseToString(response);

            if (asString == string.Empty)
                return new byte[0];

            return response.ContentTypeEncoding.GetBytes(asString);
        }

        public string ParseToString(HttpServerResponse response)
        {
            if (string.IsNullOrEmpty(response.Content))
                return string.Empty;

            return response.Content;
        }
    }
}
