using Restup.HttpMessage.Models.Contracts;
using Restup.HttpMessage.Plumbing;
using System;

namespace Restup.HttpMessage.ServerResponseParsers
{
    public class StartLineParser : IHttpResponsePartParser
    {
        public byte[] ParseToBytes(HttpServerResponse response)
        {
            return Constants.DefaultHttpEncoding.GetBytes(ParseToString(response));
        }

        public string ParseToString(HttpServerResponse response)
        {
            var version = GetHttpVersion(response.HttpVersion);
            var status = (int)response.ResponseStatus;
            var statusText = HttpCodesTranslator.Default.GetHttpStatusCodeText(status);

            return $"{version} {status} {statusText}\r\n";
        }

        private static string GetHttpVersion(Version httpVersion)
        {
            return $"HTTP/{httpVersion.Major}.{httpVersion.Minor}";
        }
    }
}
