using Devkoes.Restup.WebServer.Models.Contracts;

namespace Devkoes.Restup.WebServer.Http.RequestParsers
{
    internal abstract class HttpRequestPartParser : IHttpRequestPartParser
    {
        public bool IsFinished { get; protected set; }

        public bool IsSucceeded { get; protected set; }

        public byte[] UnparsedData { get; protected set; }

        public abstract void HandleRequestPart(byte[] stream, HttpRequest resultThisFar);
    }
}
