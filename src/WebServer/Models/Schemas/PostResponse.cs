using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Contracts;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    [RestVerb(HttpMethod.POST)]
    public struct PostResponse : IBodyRestResponse
    {
        public enum ResponseStatus : int
        {
            Created = 201,
            Conflict = 409
        };

        public object BodyData { get; }
        public ResponseStatus Status { get; }
        public string LocationRedirect { get; }

        public PostResponse(ResponseStatus status, string locationRedirectUri, object body)
        {
            Status = status;
            LocationRedirect = locationRedirectUri;
            BodyData = body;
        }

        public PostResponse(ResponseStatus status, string locationRedirectUri) : this(status, locationRedirectUri, null)
        {
        }

        public PostResponse(ResponseStatus status) : this(status, null)
        {
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
