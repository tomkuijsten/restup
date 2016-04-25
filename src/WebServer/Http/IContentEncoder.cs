using Restup.HttpMessage.Headers.Response;
using System.Threading.Tasks;

namespace Restup.Webserver.Http
{
    internal interface IContentEncoder
    {
        ContentEncodingHeader ContentEncodingHeader { get; }
        Task<byte[]> Encode(byte[] content);
    }
}