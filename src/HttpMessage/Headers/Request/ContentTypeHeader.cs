using Restup.HttpMessage.Models.Contracts;

namespace Restup.HttpMessage.Headers.Request
{
    public class ContentTypeHeader : HttpSingleQuantifiedHeaderBase
    {
        internal static string NAME = "Content-Type";
        internal static string CHARSET_QUANTIFIER_NAME = "charset";

        public string ContentType { get; }
        public string ContentCharset { get; }

        public ContentTypeHeader(string value, QuantifiedHeaderValue quantifiedHeaderValue)             
            : base(NAME, value, quantifiedHeaderValue)
        {
            ContentType = QuantifiedHeaderValue.HeaderValue;
            ContentCharset = QuantifiedHeaderValue.FindQuantifierValue(CHARSET_QUANTIFIER_NAME);
        }

        public override void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg)
        {
            v.Visit(this, arg);
        }
    }
}
