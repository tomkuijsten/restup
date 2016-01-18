using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Contracts;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    [RestVerb(HttpMethod.DELETE)]
    public struct DeleteResponse : IRestResponse
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

        T IRestResponse.Visit<P, T>(IRestResponseVisitor<P, T> visitor, P param)
        {
            return visitor.Visit(this, param);
        }
    }
}
