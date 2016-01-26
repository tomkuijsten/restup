using Devkoes.HttpMessage.Models.Contracts;

namespace Devkoes.HttpMessage.Headers
{
    public abstract class HttpRequestHeaderBase : HttpHeaderBase, IHttpRequestHeader
    {
        public HttpRequestHeaderBase(string name, string value) : base(name, value) { }

        public abstract void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg);
    }
}
