using Restup.HttpMessage;

namespace Restup.HttpMessage.Models.Contracts
{
    interface IHttpRequestPartParser
    {
        void HandleRequestPart(byte[] stream, MutableHttpServerRequest resultThisFar);
        byte[] UnparsedData { get; }
        bool IsFinished { get; }
        bool IsSucceeded { get; }
    }
}
