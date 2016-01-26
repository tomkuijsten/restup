using Devkoes.HttpMessage.Plumbing;
using System;

namespace Devkoes.HttpMessage.ServerRequestParsers
{
    internal class ResourceIdentifierParser : HttpRequestPartParser
    {
        public override void HandleRequestPart(byte[] stream, HttpServerRequest resultThisFar)
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
