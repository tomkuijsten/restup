using Devkoes.Restup.WebServer.Rest.Models.Contracts;

namespace Devkoes.Restup.WebServer.Models.Schemas
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
