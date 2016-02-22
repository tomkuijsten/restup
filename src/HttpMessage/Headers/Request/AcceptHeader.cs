using Devkoes.HttpMessage.Models.Contracts;
using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.HttpMessage.Plumbing;
using System.Collections.Generic;
using System.Linq;

namespace Devkoes.HttpMessage.Headers.Request
{
    public class AcceptHeader : HttpMultiQuantifiedHeaderBase
    {
        internal static string NAME = "Accept";

        public IEnumerable<MediaType> AcceptTypes { get; }

        public AcceptHeader(string value, IEnumerable<QuantifiedHeaderValue> quantifiedHeaderValues)
            : base(NAME, value, quantifiedHeaderValues)
        {
            AcceptTypes = QuantifiedHeaderValues.Select(q => HttpCodesTranslator.Default.GetMediaType(q.HeaderValue)).ToArray();
        }

        public override void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg)
        {
            v.Visit(this, arg);
        }
    }
}
