using System.Collections.Generic;
using System.Collections.Immutable;
using Restup.Webserver.Models.Contracts;

namespace Restup.Webserver.Models.Schemas
{
    internal class StatusOnlyResponse : RestResponse, IRestResponse
    {
        internal StatusOnlyResponse(int statusCode, IReadOnlyDictionary<string, string> headers) : base(statusCode, headers)
        { }

        internal StatusOnlyResponse(int statusCode) : this(statusCode, ImmutableDictionary<string, string>.Empty)
        { }
    }
}
