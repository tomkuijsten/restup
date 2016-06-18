using System.Collections.Generic;

namespace Devkoes.Restup.WebServer.Rest.Models.Contracts
{
    public interface IRestResponse
    {
        int StatusCode { get; }
        IEnumerable<IHeader> Headers { get; }
        IRestResponse addHeader(IHeader header);
        IRestResponse addHeader(string headerName, string headerValue);
    }
}
