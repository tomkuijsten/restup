using System.Collections.Generic;

namespace Restup.Webserver.Models.Contracts
{
    public interface IRestResponse
    {
        int StatusCode { get; }
        IReadOnlyDictionary<string, string> Headers { get; }
    }
}
