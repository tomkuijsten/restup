using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Contracts;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    [RestVerb(HttpMethod.POST)]
    public struct PostResponse : IContentRestResponse
    {
        public enum ResponseStatus : int
        {
            Created = 201,
            Conflict = 409
        };

        public object ContentData { get; }
        public ResponseStatus Status { get; }
        public string LocationRedirect { get; }

        public PostResponse(ResponseStatus status, string locationRedirectUri, object content)
        {
            Status = status;
            LocationRedirect = locationRedirectUri;
            ContentData = content;
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
    }
}
