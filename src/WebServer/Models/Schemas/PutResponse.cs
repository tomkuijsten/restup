using Restup.Webserver.Models.Contracts;

namespace Restup.Webserver.Models.Schemas
{
    public class PutResponse : IPutResponse
    {
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
    }
}
