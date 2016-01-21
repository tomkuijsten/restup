using Devkoes.HttpMessage.Models.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Devkoes.HttpMessage.Headers.Request
{
    public class AcceptCharsetHeader : HttpMultiQuantifiedHeaderBase
    {
        internal static string NAME = "Accept-Charset";

        public IEnumerable<string> ResponseContentEncoding { get; set; }

        public AcceptCharsetHeader(string value, IEnumerable<QuantifiedHeaderValue> quantifiedHeaderValues)
            : base(NAME, value, quantifiedHeaderValues)
        {
            ResponseContentEncoding = QuantifiedHeaderValues.Select(q => q.HeaderValue).ToArray();
        }

        public override void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg)
        {
            v.Visit(this, arg);
        }
    }
}
