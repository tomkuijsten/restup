using System.IO;
using System.IO.Compression;
using Devkoes.HttpMessage.Headers.Response;

namespace Devkoes.Restup.WebServer.Http
{
    internal class DeflateContentEncoder : CompressContentEncoder
    {
        public override ContentEncodingHeader ContentEncodingHeader { get; } = new ContentEncodingHeader("deflate");

        protected override Stream GetCompressStream(MemoryStream memoryStream)
        {
            return new DeflateStream(memoryStream, CompressionLevel.Optimal);
        }
    }
}