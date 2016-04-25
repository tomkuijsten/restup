using System.Collections.Generic;

namespace Restup.HttpMessage.Headers.Request
{
    public abstract class HttpMultiQuantifiedHeaderBase : HttpRequestHeaderBase
    {
        public IEnumerable<QuantifiedHeaderValue> QuantifiedHeaderValues { get; }

        protected HttpMultiQuantifiedHeaderBase(
            string name,
            string value,
            IEnumerable<QuantifiedHeaderValue> quantifiedHeaderValues) : base(name, value)
        {
            QuantifiedHeaderValues = quantifiedHeaderValues;
        }
    }
}
