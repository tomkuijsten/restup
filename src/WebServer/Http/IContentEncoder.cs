using System.Threading.Tasks;
using Devkoes.HttpMessage.Headers.Response;

namespace Devkoes.Restup.WebServer.Http
{
    internal interface IContentEncoder
    {
        ContentEncodingHeader ContentEncodingHeader { get; }
        Task<byte[]> Encode(byte[] content);
    }
}