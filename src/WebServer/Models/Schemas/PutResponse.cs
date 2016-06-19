
using Restup.Webserver.Models.Contracts;
using System.Collections.Generic;

namespace Restup.Webserver.Models.Schemas
{
    public class PutResponse : IPutResponse
    {

        public IEnumerable<IHeader> Headers { get; }

        public enum ResponseStatus : int
        {
            OK = 200,
            NoContent = 204,
            NotFound = 404
        };

        public ResponseStatus Status { get; }

        public object ContentData { get; set; }

        public PutResponse(ResponseStatus status, object content)
        {
            Headers = new List<IHeader>();
            Status = status;
            ContentData = content;
        }

        public PutResponse(ResponseStatus status) : this(status, null)
        {
        }

        public static PutResponse CreateNotFound()
        {
            return new PutResponse(ResponseStatus.NotFound, null);
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
