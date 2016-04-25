using Restup.HttpMessage.Models.Schemas;
using System.Collections.Generic;

namespace Restup.HttpMessage.Headers.Response
{
    public class AllowHeader : HttpHeaderBase
    {
        internal static string NAME = "Allow";

        public IEnumerable<HttpMethod> Allows { get; }

        public AllowHeader(IEnumerable<HttpMethod> allows) : base(NAME, string.Join(";", allows))
        {
            Allows = allows;
        }
    }
}
