using Restup.HttpMessage.Headers.Response;
using System.IO;
using System.IO.Compression;

namespace Restup.Webserver.Http
{
    internal class GzipContentEncoder : CompressContentEncoder
    {
        public override ContentEncodingHeader ContentEncodingHeader { get; } = new ContentEncodingHeader("gzip");

        protected override Stream GetCompressStream(MemoryStream memoryStream)
        {
            return new GZipStream(memoryStream, CompressionLevel.Optimal);
        }
    }
}