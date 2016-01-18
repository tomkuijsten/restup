using System.Collections.Generic;

namespace Devkoes.HttpMessage.Headers
{
    public abstract class HttpMultiQuantifiedHeaderBase : HttpHeaderBase
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
