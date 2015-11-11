using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Contracts;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    [RestVerb(RestVerb.PUT)]
    public struct PutResponse : IRestResponse
    {
        public enum ResponseStatus : int
        {
            OK = 200,
            NoContent = 204,
            NotFound = 404
        };

        public ResponseStatus Status { get; }

        public PutResponse(ResponseStatus status)
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

        public T Visit<P, T>(IRestResponseVisitor<P, T> visitor, P param)
        {
            return visitor.Visit(this, param);
        }
    }
}
