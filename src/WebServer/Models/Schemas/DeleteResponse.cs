using Restup.Webserver.Models.Contracts;
using System.Collections.Generic;

namespace Restup.Webserver.Models.Schemas
{
    public class DeleteResponse : IDeleteResponse
    {

        public IEnumerable<IHeader> Headers { get; }

        public enum ResponseStatus : int
        {
            OK = 200,
            NoContent = 204,
            NotFound = 404
        };

        public ResponseStatus Status { get; }

        public DeleteResponse(ResponseStatus status)
        {
            Status = status;
            this.Headers = new List<IHeader>();
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
