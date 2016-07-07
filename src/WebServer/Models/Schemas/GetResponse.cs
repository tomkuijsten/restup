using System.Collections.Generic;
using System.Collections.Immutable;
using Restup.Webserver.Models.Contracts;

namespace Restup.Webserver.Models.Schemas
{
    public class GetResponse : RestResponse, IGetResponse
    {
        public enum ResponseStatus : int
        {
            OK = 200,
            NotFound = 404
        };

        public ResponseStatus Status { get; }
        public object ContentData { get; }

        public GetResponse(ResponseStatus status, IReadOnlyDictionary<string, string> headers, object data) : base((int)status, headers)
        {
            Status = status;
            ContentData = data;
        }

        public GetResponse(ResponseStatus status) : this(status, ImmutableDictionary<string, string>.Empty, null) { }
        public GetResponse(ResponseStatus status, IReadOnlyDictionary<string, string> headers) : this(status, headers, null) { }
        public GetResponse(ResponseStatus status, object data) : this(status, ImmutableDictionary<string, string>.Empty, data) { }
    }
}
