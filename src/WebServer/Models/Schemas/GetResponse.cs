using Devkoes.Restup.WebServer.Models.Contracts;
using Devkoes.Restup.WebServer.Attributes;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    [RestVerb(RestVerb.GET)]
    public struct GetResponse : IBodyRestResponse
    {
        public enum ResponseStatus : int {
            OK = 200,
            NotFound = 404
        };

        public ResponseStatus Status { get; }
        public object BodyData { get; }

        public GetResponse(ResponseStatus status, object data)
        {
            Status = status;
            BodyData = data;
        }

        public int StatusCode
        {
            get
            {
                return (int)Status;
            }
        }

        public void Accept(IRestResponseVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
