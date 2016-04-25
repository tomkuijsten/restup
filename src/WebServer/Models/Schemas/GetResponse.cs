using Restup.Webserver.Models.Contracts;

namespace Restup.Webserver.Models.Schemas
{
    public class GetResponse : IGetResponse
    {
        public enum ResponseStatus : int
        {
            OK = 200,
            NotFound = 404
        };

        public ResponseStatus Status { get; }
        public object ContentData { get; }

        public GetResponse(ResponseStatus status) : this(status, null) { }

        public GetResponse(ResponseStatus status, object data)
        {
            Status = status;
            ContentData = data;
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
