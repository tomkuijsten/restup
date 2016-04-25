using Restup.HttpMessage.Models.Schemas;
using System.Collections.Generic;

namespace Restup.Webserver.Models.Schemas
{
    internal class MethodNotAllowedResponse : StatusOnlyResponse
    {
        public IEnumerable<HttpMethod> Allows { get; }

        internal MethodNotAllowedResponse(IEnumerable<HttpMethod> allows) : base(405)
        {
            Allows = allows;
        }
    }
}
