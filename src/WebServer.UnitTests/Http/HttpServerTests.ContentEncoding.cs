using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Restup.HttpMessage.Headers.Response;
using Restup.Webserver.UnitTests.TestHelpers;
using ContentTypeHeader = Restup.HttpMessage.Headers.Request.ContentTypeHeader;

namespace Restup.Webserver.UnitTests.Http
{
    [TestClass]
    public class HttpServerTests_ContentEncoding
    {
        [TestMethod]
        public void WithGzipAcceptEncoding()
        {
            new FluentHttpServerTests()
                .Given
                    .ListeningOnDefaultRoute()
                .When
                    .RequestHasArrived("/Get", new[] { "gzip" }, Encoding.UTF8.GetBytes("tom"))
                .Then
                    .AssertRouteHandlerReceivedRequest()
                    .AssertLastResponse<ContentEncodingHeader, string>(x => x.Encoding, "gzip")
                    .AssertLastResponse(x => DecompressGzipContent(x.Content), "tom");
        }

        [TestMethod]
        public void WithDeflateAcceptEncoding()
        {
            new FluentHttpServerTests()
               .Given
                   .ListeningOnDefaultRoute()
               .When
                   .RequestHasArrived("/Get", new[] { "deflate" }, Encoding.UTF8.GetBytes("tom"))
               .Then
                   .AssertRouteHandlerReceivedRequest()
                   .AssertLastResponse<ContentEncodingHeader, string>(x => x.Encoding, "deflate")
                   .AssertLastResponse(x => DecompressDeflateContent(x.Content), "tom");
        }

        [TestMethod]
        public void WithNoEncoding()
        {
            new FluentHttpServerTests()
               .Given
                   .ListeningOnDefaultRoute()
               .When
                   .RequestHasArrived("/Get", content: Encoding.UTF8.GetBytes("tom"))
               .Then
                   .AssertRouteHandlerReceivedRequest()
                   .AssertLastResponse(x => Encoding.UTF8.GetString(x.Content), "tom")
                   .AssertLastResponse(x => x.Headers.OfType<ContentTypeHeader>().FirstOrDefault(), null);
        }

        private static string DecompressGzipContent(byte[] content)
        {
            return DecompressContent(content, x => new GZipStream(x, CompressionMode.Decompress));
        }

        private static string DecompressDeflateContent(byte[] content)
        {
            return DecompressContent(content, x => new DeflateStream(x, CompressionMode.Decompress));
        }

        private static string DecompressContent<T>(byte[] content, Func<MemoryStream, T> createStreamFunc) where T : Stream
        {
            using (var stream = createStreamFunc(new MemoryStream(content)))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}
