using Devkoes.Restup.WebServer.Models.Contracts;
using System.Collections.Generic;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    internal class MethodNotAllowedResponse : StatusOnlyResponse
    {
        public IEnumerable<HttpMethod> Allows { get; }

        internal MethodNotAllowedResponse(IEnumerable<HttpMethod> allows) : base(405)
        {
            Allows = allows;
        }

        public override T Visit<P, T>(IRestResponseVisitor<P, T> visitor, P param)
        {
            return visitor.Visit(this, param);
        }
    }
}
