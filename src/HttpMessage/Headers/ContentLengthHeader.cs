using Devkoes.HttpMessage.Models.Contracts;

namespace Devkoes.HttpMessage.Headers
{
    public class ContentLengthHeader : HttpHeaderBase
    {
        internal static string NAME = "Content-Length";

        public int Length { get; set; }

        public ContentLengthHeader(string value) : base(NAME, value)
        {
            Length = int.Parse(value);
        }

        public override void Visit<T>(IHttpHeaderVisitor<T> v, T arg)
        {
            v.Visit(this, arg);
        }
    }
}
