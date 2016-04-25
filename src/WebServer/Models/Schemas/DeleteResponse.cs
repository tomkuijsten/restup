using Restup.Webserver.Models.Contracts;

namespace Restup.Webserver.Models.Schemas
{
    public class DeleteResponse : IDeleteResponse
    {
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
