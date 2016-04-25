using Restup.Webserver.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Restup.Webserver.UnitTests.File
{
    public class MockFileSystem : IFileSystem
    {
        private readonly Dictionary<string, MockFile> _files = new Dictionary<string, MockFile>(StringComparer.OrdinalIgnoreCase);
        private readonly bool _pathExists;

        public MockFileSystem(bool pathExists)
        {
            _pathExists = pathExists;
        }

        public void AddFile(string path, MockFile file)
        {
            _files[path] = file;
        }

        public bool Exists(string absoluteBasePathUri)
        {
            return _pathExists;
        }

        public Task<IFile> GetFileFromPathAsync(string path)
        {
            MockFile file;
            if (!_files.TryGetValue(path, out file))
                throw new FileNotFoundException();

            return Task.FromResult((IFile)file);
        }
    }
}