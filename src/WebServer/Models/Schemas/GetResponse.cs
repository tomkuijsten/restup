using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Contracts;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    [RestVerb(HttpMethod.GET)]
    public struct GetResponse : IBodyRestResponse
    {
        public enum ResponseStatus : int
        {
            OK = 200,
            NotFound = 404
        };

        public ResponseStatus Status { get; }
        public object BodyData { get; }

        public GetResponse(ResponseStatus status) : this(status, null) { }

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

        T IRestResponse.Visit<P, T>(IRestResponseVisitor<P, T> visitor, P param)
        {
            return visitor.Visit(this, param);
        }
    }
}
