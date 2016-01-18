using Devkoes.HttpMessage.Models.Contracts;

namespace Devkoes.HttpMessage.RequestParsers
{
    internal abstract class HttpRequestPartParser : IHttpRequestPartParser
    {
        public bool IsFinished { get; protected set; }

        public bool IsSucceeded { get; protected set; }

        public byte[] UnparsedData { get; protected set; }

        public abstract void HandleRequestPart(byte[] stream, HttpRequest resultThisFar);
    }
}
