using Restup.HttpMessage.Headers.Response;
using System.IO;
using System.IO.Compression;

namespace Restup.Webserver.Http
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