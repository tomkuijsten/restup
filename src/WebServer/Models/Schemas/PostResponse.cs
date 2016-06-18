using Devkoes.Restup.WebServer.Rest.Models.Contracts;
using System.Collections.Generic;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    public class PostResponse : IPostResponse
    {

        public IEnumerable<IHeader> Headers { get; }

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
            Headers = new List<IHeader>();

            Status = status;
            LocationRedirect = locationRedirectUri;
            ContentData = content;
        }

        public PostResponse(ResponseStatus status, string locationRedirectUri) : this(status, locationRedirectUri, null)
        {
            Headers = new List<IHeader>();
        }

        public PostResponse(ResponseStatus status) : this(status, null)
        {
            Headers = new List<IHeader>();
        }

        public int StatusCode
        {
            get
            {
                return (int)Status;
            }
        }

        public IRestResponse addHeader(IHeader headerToAdd)
        {
            ((List<IHeader>)Headers).Add(headerToAdd);

            return this;
        }

        public IRestResponse addHeader(string headerName, string headerValue)
        {
            return addHeader(new Header(headerName, headerValue));
        }
    }
}
