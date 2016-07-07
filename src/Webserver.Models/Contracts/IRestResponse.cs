using System.Collections.Generic;

namespace Restup.Webserver.Models.Contracts
{
    public interface IRestResponse
    {
        int StatusCode { get; }
        IEnumerable<IHeader> Headers { get; }
        IRestResponse addHeader(IHeader header);
        IRestResponse addHeader(string headerName, string headerValue);
    }
}
