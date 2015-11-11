using Devkoes.Restup.WebServer.Models.Contracts;
using System.Collections.Generic;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    public class MethodNotAllowedResponse : StatusOnlyResponse
    {
        public IEnumerable<RestVerb> Allows { get; }

        public MethodNotAllowedResponse(IEnumerable<RestVerb> allows) : base(405)
        {
            Allows = allows;
        }

        public override T Visit<P, T>(IRestResponseVisitor<P, T> visitor, P param)
        {
            return visitor.Visit(this, param);
        }
    }
}
