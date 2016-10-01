using System.Threading.Tasks;

namespace Restup.Webserver.File
{
    public interface IFileSystem
    {
        bool Exists(string absoluteBasePathUri);
        Task<IFile> GetFileFromPathAsync(string path);
    }
}