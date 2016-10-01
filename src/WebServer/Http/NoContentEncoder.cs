using Restup.HttpMessage.Headers.Response;
using System.Threading.Tasks;

namespace Restup.Webserver.Http
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