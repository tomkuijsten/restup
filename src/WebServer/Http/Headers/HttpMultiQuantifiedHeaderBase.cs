using System.Collections.Generic;

namespace Devkoes.Restup.WebServer.Http.Headers
{
    internal abstract class HttpMultiQuantifiedHeaderBase : HttpHeaderBase
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
