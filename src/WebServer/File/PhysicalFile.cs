using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace Devkoes.Restup.WebServer.File
{
    public class PhysicalFile : IFile
    {
        private readonly StorageFile _storageFile;

        public string ContentType => _storageFile.ContentType;

        public PhysicalFile(StorageFile storageFile)
        {
            _storageFile = storageFile;
        }

        public Task<Stream> OpenStreamForReadAsync()
        {
            return _storageFile.OpenStreamForReadAsync();
        }
    }
}