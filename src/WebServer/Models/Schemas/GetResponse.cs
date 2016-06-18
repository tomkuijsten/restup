using Devkoes.Restup.WebServer.Models.Contracts;
using Devkoes.Restup.WebServer.Rest.Models.Contracts;
using System.Collections.Generic;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    public class GetResponse : IGetResponse
    {
        public IEnumerable<IHeader> Headers { get; }

        public enum ResponseStatus : int
        {
            OK = 200,
            NotFound = 404
        };

        public ResponseStatus Status { get; }
        public object ContentData { get; }

        public GetResponse(ResponseStatus status) : this(status, null)
        {
            Headers = new List<IHeader>();
        }

        public GetResponse(ResponseStatus status, object data)
        {
            Status = status;
            ContentData = data;
            Headers = new List<IHeader>();
        }

        public int StatusCode
        {
            get
            {
                return (int)Status;
            }
        }

        public IRestResponse addHeader(IHeader headerToAdd)
        {
            ((List<IHeader>)Headers).Add(headerToAdd);

            return this;
        }

        public IRestResponse addHeader(string headerName, string headerValue)
        {
            return addHeader(new Header(headerName, headerValue));
        }
    }
}
