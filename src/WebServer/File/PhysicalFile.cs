using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace Restup.Webserver.File
{
    public class PhysicalFile : IFile
    {
        private readonly StorageFile _storageFile;

        public string ContentType => _storageFile.ContentType;

        public string Extension => _storageFile.FileType;

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