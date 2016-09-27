using System.Collections.Generic;
using System.Linq;
using Restup.HttpMessage.Models.Contracts;

namespace Restup.HttpMessage.Headers.Request
{
    public class AccessControlRequestHeadersHeader : HttpRequestHeaderBase
    {
        internal static string NAME = "Access-Control-Request-Headers";

        public IEnumerable<string> Headers { get; }

        public AccessControlRequestHeadersHeader(string value) : base(NAME, value)
        {
            Headers = Parse(value);
        }

        private static IEnumerable<string> Parse(string value)
        {
            return value.Split(',').Select(x => x.Trim());
        }

        public override void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg)
        {
            v.Visit(this, arg);
        }
    }
}