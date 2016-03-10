using System.Threading.Tasks;
using Devkoes.HttpMessage.Headers.Response;

namespace Devkoes.Restup.WebServer.Http
{
    internal class NoContentEncoder : IContentEncoder
    {
        public ContentEncodingHeader ContentEncodingHeader { get; } = null;

        public Task<byte[]> Encode(byte[] content)
        {
            return Task.FromResult(content);
        }
    }
}