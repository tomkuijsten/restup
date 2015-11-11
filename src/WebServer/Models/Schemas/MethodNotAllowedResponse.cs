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

        public override T Visit<T>(IRestResponseVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
