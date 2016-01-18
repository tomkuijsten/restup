using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.HttpMessage.Plumbing;
using System;

namespace Devkoes.HttpMessage.RequestParsers
{
    internal class MethodParser : HttpRequestPartParser
    {
        public override void HandleRequestPart(byte[] stream, HttpServerRequest resultThisFar)
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
