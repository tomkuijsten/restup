using Devkoes.Restup.WebServer.Http.RequestFactory;

namespace Devkoes.Restup.WebServer.Models.Contracts
{
    interface IRequestPipelinePart
    {
        void HandleRequestPart(byte[] stream, HttpRequest resultThisFar);
        byte[] UnparsedData { get; }
        bool IsFinished { get; }
        bool IsSucceeded { get; }
    }
}
