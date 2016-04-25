using Restup.Webserver.File;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Restup.Webserver.UnitTests.File
{
    public class MockFile : IFile
    {
        public string Content { get; }
        public string ContentType { get; }
        public string Extension { get; }

        public MockFile(string content, string contentType, string extension)
        {
            Content = content;
            ContentType = contentType;
            Extension = extension;
        }

        public Task<Stream> OpenStreamForReadAsync()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(Content));

            return Task.FromResult((Stream)stream);
        }
    }
}