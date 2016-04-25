using Restup.HttpMessage.Models.Contracts;
using Restup.HttpMessage.Plumbing;
using System.Text;

namespace Restup.HttpMessage.ServerResponseParsers
{
    internal class HeadersParser : IHttpResponsePartParser
    {
        public byte[] ParseToBytes(HttpServerResponse response)
        {
            return Constants.DefaultHttpEncoding.GetBytes(ParseToString(response));
        }

        public string ParseToString(HttpServerResponse response)
        {
            var headersTextBuilder = new StringBuilder();
            foreach (var header in response.Headers)
            {
                headersTextBuilder.Append($"{header.Name}: {header.Value}\r\n");
            }

            headersTextBuilder.Append("\r\n");

            return headersTextBuilder.ToString();
        }
    }
}
