using Devkoes.Restup.WebServer.Models.Schemas;
using System;

namespace Devkoes.Restup.WebServer.Http.RequestFactory
{
    internal class MethodParser : RequestPipelinePart
    {
        public override void HandleRequestPart(byte[] stream, HttpRequest resultThisFar)
        {
            var word = ReadNextWord(stream);

            if (word.WordFound)
            {
                resultThisFar.Method = GetMethod(word.Word);
                UnparsedData = word.RemainingBytes;
                Finished = true;
            }
        }

        private RestVerb GetMethod(string method)
        {
            method = method.ToUpper();

            RestVerb methodVerb = RestVerb.Unsupported;

            Enum.TryParse<RestVerb>(method, out methodVerb);

            return methodVerb;
        }
    }
}
