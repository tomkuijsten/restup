using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using Restup.Webserver.Models.Contracts;

namespace Restup.Webserver.Models.Schemas
{
    public class PostResponse : RestResponse, IPostResponse
    {
        public enum ResponseStatus : int
        {
            Created = 201,
            Conflict = 409
        };

        public object ContentData { get; }
        public ResponseStatus Status { get; }
        public string LocationRedirect { get; }

        public PostResponse(ResponseStatus status, string locationRedirectUri, object content, IReadOnlyDictionary<string, string> headers) : base((int)status, headers)
        {
            Status = status;
            LocationRedirect = locationRedirectUri;
            ContentData = content;
        }

        public PostResponse(ResponseStatus status, string locationRedirectUri, object content) : this(status, locationRedirectUri, content, ImmutableDictionary<string, string>.Empty)
        { }

        public PostResponse(ResponseStatus status, string locationRedirectUri, IReadOnlyDictionary<string, string> headers) : this(status, locationRedirectUri, null, headers)
        { }

        public PostResponse(ResponseStatus status, string locationRedirectUri) : this(status, locationRedirectUri, null, ImmutableDictionary<string, string>.Empty)
        { }

        public PostResponse(ResponseStatus status) : this(status, null, null, ImmutableDictionary<string, string>.Empty)
        { }
    }
}
