using Devkoes.HttpMessage.Models.Contracts;
using Devkoes.HttpMessage.Models.Schemas;
using System.Collections.Generic;
using System.Linq;

namespace Devkoes.HttpMessage.Headers
{
    public class AcceptHeader : HttpMultiQuantifiedHeaderBase
    {
        internal static string NAME = "Accept";

        public MediaType AcceptType { get; set; }

        public AcceptHeader(string value, IEnumerable<QuantifiedHeaderValue> quantifiedHeaderValues)
            : base(NAME, value, quantifiedHeaderValues)
        {
            AcceptType = HttpCodesTranslator.Default.GetMediaType(QuantifiedHeaderValues.First().HeaderValue);
        }

        public override void Visit<T>(IHttpHeaderVisitor<T> v, T arg)
        {
            v.Visit(this, arg);
        }
    }
}
