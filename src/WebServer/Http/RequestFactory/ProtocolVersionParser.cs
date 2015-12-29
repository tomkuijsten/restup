using Devkoes.Restup.WebServer.Models.Schemas;

namespace Devkoes.Restup.WebServer.Http.RequestFactory
{
    internal class ProtocolVersionParser : RequestPipelinePart
    {
        public override void HandleRequestPart(byte[] stream, HttpRequest resultThisFar)
        {
            var word = ReadNextWord(stream);

            if (word.WordFound)
            {
                resultThisFar.HttpVersion = word.Word;
                UnparsedData = word.RemainingBytes;
                Finished = true;
            }
        }
    }
}
