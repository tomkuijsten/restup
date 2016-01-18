using Devkoes.HttpMessage.Models.Contracts;

namespace Devkoes.HttpMessage.Headers
{
    public class UntypedHeader : HttpHeaderBase
    {
        public UntypedHeader(string name, string value) : base(name, value) { }

        public override void Visit<T>(IHttpHeaderVisitor<T> v, T arg)
        {
            v.Visit(this, arg);
        }
    }
}
