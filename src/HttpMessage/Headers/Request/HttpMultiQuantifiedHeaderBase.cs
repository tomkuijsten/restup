using System.Collections.Generic;

namespace Devkoes.HttpMessage.Headers.Request
{
    public abstract class HttpMultiQuantifiedHeaderBase : HttpRequestHeaderBase
    {
        public IEnumerable<QuantifiedHeaderValue> QuantifiedHeaderValues { get; set; }

        public HttpMultiQuantifiedHeaderBase(
            string name,
            string value,
            IEnumerable<QuantifiedHeaderValue> quantifiedHeaderValues) : base(name, value)
        {
            QuantifiedHeaderValues = quantifiedHeaderValues;
        }
    }
}
