using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Restup.HttpMessage.Headers.Request;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Restup.Webserver.UnitTests.Http
{
    [TestClass]
    public class HttpServerContentEncodingTests
    {
        [TestMethod]
        public void WithGzipAcceptEncoding()
        {
            new HttpServerFluentTests()
                .Given
                    .ListeningOnDefaultRoute()
                .When
                    .RequestHasArrived("/Get", new[] { "gzip" }, Encoding.UTF8.GetBytes("tom"))
                .Then
                    .AssertRouteHandlerReceivedRequest()
                    .AssertLastResponse(x => x.Encoding, "gzip")
                    .AssertLastResponse(x => DecompressGzipContent(x.Content), "tom");
        }

        [TestMethod]
        public void WithDeflateAcceptEncoding()
        {
            new HttpServerFluentTests()
               .Given
                   .ListeningOnDefaultRoute()
               .When
                   .RequestHasArrived("/Get", new[] { "deflate" }, Encoding.UTF8.GetBytes("tom"))
               .Then
                   .AssertRouteHandlerReceivedRequest()
                   .AssertLastResponse(x => x.Encoding, "deflate") // used http://www.txtwizard.net/compression to compress "tom", represented here in base64
                   .AssertLastResponse(x => DecompressDeflateContent(x.Content), "tom");
        }

        [TestMethod]
        public void WithNoEncoding()
        {
            new HttpServerFluentTests()
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
