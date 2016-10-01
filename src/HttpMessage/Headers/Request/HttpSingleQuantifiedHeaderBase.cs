namespace Restup.HttpMessage.Headers.Request
{
    public abstract class HttpSingleQuantifiedHeaderBase : HttpRequestHeaderBase
    {
        public QuantifiedHeaderValue QuantifiedHeaderValue { get; }

        protected HttpSingleQuantifiedHeaderBase(string name, string value, QuantifiedHeaderValue quantifiedHeaderValue)
            : base(name, value)
        {
            QuantifiedHeaderValue = quantifiedHeaderValue;
        }
    }
}
