using System.Collections.Generic;
using Restup.Webserver.Models.Contracts;

namespace Restup.Webserver.Models.Schemas
{
    public class RestResponse : IRestResponse
    {
        public int StatusCode { get; }
        public IReadOnlyDictionary<string, string> Headers { get; }

        public RestResponse(int statusCode, IReadOnlyDictionary<string, string> headers)
        {
            StatusCode = statusCode;
            Headers = headers;
        }
    }
}