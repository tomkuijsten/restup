using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Http.RequestFactory;
using Devkoes.Restup.WebServer.Models.Contracts;
using System.Linq;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    internal abstract class RequestPipelinePart : IRequestPipelinePart
    {
        protected struct NextWord
        {
            public string Word { get; set; }
            public byte[] RemainingBytes { get; set; }
            public bool WordFound { get; set; }
        }

        public bool Finished { get; protected set; }

        public byte[] UnparsedData { get; protected set; }

        public abstract void HandleRequestPart(byte[] stream, HttpRequest resultThisFar);

        protected NextWord ReadNextWord(byte[] stream)
        {
            for (int i = 0; i < stream.Length; i++)
            {
                byte currentByte = stream[i];
                if (currentByte == HttpConstants.SpaceByte)
                {
                    return new NextWord()
                    {
                        Word = HttpConstants.DefaultHttpMessageCharset.GetString(stream.Take(i).ToArray()),
                        RemainingBytes = stream.Skip(i + 1).ToArray(),
                        WordFound = true
                    };
                }
                else if (currentByte == HttpConstants.CRByte)
                {
                    byte next = stream.Length > (i + 1) ? stream[i + 1] : (byte)0;
                    if (next == HttpConstants.LFByte)
                    {
                        return new NextWord()
                        {
                            Word = HttpConstants.DefaultHttpMessageCharset.GetString(stream.Take(i).ToArray()),
                            RemainingBytes = stream.Skip(i + 2).ToArray(),
                            WordFound = true
                        };

                    }
                }
            }

            return new NextWord();
        }
    }
}
