using Devkoes.HttpMessage.Models.Contracts;
using System;

namespace Devkoes.HttpMessage.ServerResponseParsers
{
    internal class ContentParser : IHttpResponsePartParser
    {
        public byte[] ParseToBytes(HttpServerResponse response)
        {
            throw new NotImplementedException();
        }

        public string ParseToString(HttpServerResponse response)
        {
            return string.Empty;
        }
    }
}
