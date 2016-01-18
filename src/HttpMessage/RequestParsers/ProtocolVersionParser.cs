namespace Devkoes.HttpMessage.RequestParsers
{
    internal class ProtocolVersionParser : HttpRequestPartParser
    {
        public override void HandleRequestPart(byte[] stream, HttpRequest resultThisFar)
        {
            var word = stream.ReadNextWord();

            if (word.WordFound)
            {
                resultThisFar.HttpVersion = word.Word;
                UnparsedData = word.RemainingBytes;
                IsFinished = true;
                IsSucceeded = true;
            }
        }
    }
}
