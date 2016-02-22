using Devkoes.HttpMessage.Models.Contracts;

namespace Devkoes.HttpMessage.Headers.Request
{
    public class ContentLengthHeader : HttpRequestHeaderBase
    {
        internal static string NAME = "Content-Length";

        public int Length { get; }

        public ContentLengthHeader(string value) : base(NAME, value)
        {
            Length = int.Parse(value);
        }

        public override void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg)
        {
            v.Visit(this, arg);
        }
    }
}
