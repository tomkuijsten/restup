using Devkoes.HttpMessage.Headers;
using Devkoes.HttpMessage.Headers.Request;

namespace Devkoes.HttpMessage.Models.Contracts
{
    public interface IHttpRequestHeaderVisitor<T>
    {
        void Visit(UntypedRequestHeader uh, T arg);
        void Visit(ContentLengthHeader uh, T arg);
        void Visit(AcceptHeader uh, T arg);
        void Visit(ContentTypeHeader uh, T arg);
        void Visit(AcceptCharsetHeader uh, T arg);
    }
}
