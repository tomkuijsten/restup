using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Contracts;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    [RestVerb(RestVerb.PUT)]
    public struct PutResponse : IBodyRestResponse
    {
        public enum ResponseStatus : int
        {
            OK = 200,
            NoContent = 204,
            NotFound = 404
        };

        public ResponseStatus Status { get; }

        public object BodyData { get; set; }

        public PutResponse(ResponseStatus status, object body)
        {
            Status = status;
            BodyData = body;
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

        public T Visit<P, T>(IRestResponseVisitor<P, T> visitor, P param)
        {
            return visitor.Visit(this, param);
        }
    }
}
