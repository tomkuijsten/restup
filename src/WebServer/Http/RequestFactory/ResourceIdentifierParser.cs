using Devkoes.Restup.WebServer.Models.Schemas;
using System;

namespace Devkoes.Restup.WebServer.Http.RequestFactory
{
    internal class ResourceIdentifierParser : RequestPipelinePart
    {
        public override void HandleRequestPart(byte[] stream, HttpRequest resultThisFar)
        {
            var word = ReadNextWord(stream);

            if (word.WordFound)
            {
                resultThisFar.Uri = new Uri(word.Word, UriKind.Relative);
                UnparsedData = word.RemainingBytes;
                IsFinished = true;
                IsSucceeded = true;
            }
        }
    }
}
