using System.Collections.Generic;
using Devkoes.HttpMessage.Models.Schemas;

namespace Devkoes.Restup.WebServer.Models.Schemas
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
