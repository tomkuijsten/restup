using Devkoes.HttpMessage.Models.Contracts;
using System;

namespace Devkoes.HttpMessage.ServerResponseParsers
{
    internal class HeadersParser : IHttpResponsePartParser
    {
        public byte[] ParseToBytes(HttpServerResponse response)
        {
            throw new NotImplementedException();
        }

        public string ParseToString(HttpServerResponse response)
        {
            return "\r\n";
        }
    }
}
