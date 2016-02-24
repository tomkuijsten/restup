using System.IO;
using System.Text;
using System.Threading.Tasks;
using Devkoes.Restup.WebServer.File;

namespace Devkoes.Restup.WebServer.UnitTests.File
{
    public class MockFile : IFile
    {
        public string Content { get; }
        public string ContentType { get; }

        public MockFile(string content, string contentType)
        {
            Content = content;
            ContentType = contentType;
        }

        public Task<Stream> OpenStreamForReadAsync()
        {
            var encoding = Encoding.GetEncoding(ContentType);
            var stream = new MemoryStream(encoding.GetBytes(Content));

            return Task.FromResult((Stream)stream);
        }
    }
}