using System.Collections.Generic;
using System.Collections.Immutable;
using Restup.Webserver.Models.Contracts;

namespace Restup.Webserver.Models.Schemas
{
    public class PutResponse : RestResponse, IPutResponse
    {
        public enum ResponseStatus : int
        {
            OK = 200,
            NoContent = 204,
            NotFound = 404
        };

        public ResponseStatus Status { get; }

        public object ContentData { get; set; }

        public PutResponse(ResponseStatus status, object content, IReadOnlyDictionary<string, string> headers) : base((int)status, headers)
        {
            Status = status;
            ContentData = content;
        }

        public PutResponse(ResponseStatus status, object content) : this(status, content, ImmutableDictionary<string, string>.Empty)
        { }

        public PutResponse(ResponseStatus status, IReadOnlyDictionary<string, string> headers) : this(status, null, headers)
        { }

        public PutResponse(ResponseStatus status) : this(status, null, ImmutableDictionary<string, string>.Empty)
        { }

        public static PutResponse CreateNotFound()
        {
            return new PutResponse(ResponseStatus.NotFound, null);
        }
    }
}
