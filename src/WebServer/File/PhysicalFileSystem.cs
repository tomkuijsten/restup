using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace Restup.Webserver.File
{
    public class PhysicalFileSystem : IFileSystem
    {
        public bool Exists(string absoluteBasePathUri)
        {
            return Directory.Exists(absoluteBasePathUri);
        }

        public async Task<IFile> GetFileFromPathAsync(string path)
        {
            var storageFile = await StorageFile.GetFileFromPathAsync(path);

            return new PhysicalFile(storageFile);
        }
    }
}