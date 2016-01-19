using Devkoes.HttpMessage.Models.Contracts;
using Devkoes.HttpMessage.Plumbing;
using System;

namespace Devkoes.HttpMessage.ServerResponseParsers
{
    public class StartLineParser : IHttpResponsePartParser
    {
        public byte[] ParseToBytes(HttpServerResponse response)
        {
            throw new NotImplementedException();
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
