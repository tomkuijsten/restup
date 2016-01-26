using Devkoes.HttpMessage.Models.Contracts;

namespace Devkoes.HttpMessage.Headers
{
    public class UntypedRequestHeader : HttpRequestHeaderBase
    {
        public UntypedRequestHeader(string name, string value) : base(name, value) { }

        public override void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg)
        {
            v.Visit(this, arg);
        }
    }
}
