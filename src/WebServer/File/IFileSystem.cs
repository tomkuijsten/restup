using System.Threading.Tasks;

namespace Devkoes.Restup.WebServer.File
{
    public interface IFileSystem
    {
        bool Exists(string absoluteBasePathUri);
        Task<IFile> GetFileFromPathAsync(string path);
    }
}