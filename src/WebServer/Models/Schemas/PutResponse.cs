using Devkoes.Restup.WebServer.Attributes;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    [RestVerb(RestVerb.PUT)]
    public struct PutResponse
    {
        public enum GetResponseStatus : int
        {
            OK = 200,
            NoContent = 204,
            NotFound = 404
        };

        public GetResponseStatus Status { get; }

        public PutResponse(GetResponseStatus status)
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
