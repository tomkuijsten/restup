using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Contracts;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    [RestVerb(RestVerb.PUT)]
    public struct PutResponse : IRestResponse
    {
        public object Data { get; }

        public enum PutResponseStatus : int
        {
            OK = 200,
            NoContent = 204,
            NotFound = 404
        };

        public PutResponseStatus Status { get; }

        public PutResponse(PutResponseStatus status)
        {
            Status = status;
            Data = null;
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
