using System.IO;
using System.Threading.Tasks;

namespace Restup.Webserver.File
{
    public interface IFile
    {
        string ContentType { get; }
        string Extension { get; }
        Task<Stream> OpenStreamForReadAsync();
    }
}