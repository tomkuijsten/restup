using System;

namespace Devkoes.HttpMessage.RequestParsers
{
    internal class ResourceIdentifierParser : HttpRequestPartParser
    {
        public override void HandleRequestPart(byte[] stream, HttpRequest resultThisFar)
        {
            var word = stream.ReadNextWord();

            if (word.WordFound)
            {
                resultThisFar.Uri = new Uri(word.Word, UriKind.RelativeOrAbsolute);
                UnparsedData = word.RemainingBytes;
                IsFinished = true;
                IsSucceeded = true;
            }
        }
    }
}
