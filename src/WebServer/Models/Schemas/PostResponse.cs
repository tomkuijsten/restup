using Restup.Webserver.Models.Contracts;

namespace Restup.Webserver.Models.Schemas
{
    public class PostResponse : IPostResponse
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
