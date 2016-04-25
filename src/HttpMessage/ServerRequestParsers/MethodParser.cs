using Restup.HttpMessage.Models.Schemas;
using Restup.HttpMessage.Plumbing;
using System;

namespace Restup.HttpMessage.ServerRequestParsers
{
    internal class MethodParser : HttpRequestPartParser
    {
        public override void HandleRequestPart(byte[] stream, MutableHttpServerRequest resultThisFar)
        {
            var word = stream.ReadNextWord();

            if (word.WordFound)
            {
                resultThisFar.Method = GetMethod(word.Word);
                UnparsedData = word.RemainingBytes;
                IsFinished = true;
                IsSucceeded = true;
            }
        }

        private HttpMethod GetMethod(string method)
        {
            method = method.ToUpper();

            HttpMethod methodVerb = HttpMethod.Unsupported;

            Enum.TryParse<HttpMethod>(method, out methodVerb);

            return methodVerb;
        }
    }
}
