using Devkoes.HttpMessage.Headers;

namespace Devkoes.HttpMessage.Models.Contracts
{
    public interface IHttpHeaderVisitor<T>
    {
        void Visit(UntypedHeader uh, T arg);
        void Visit(ContentLengthHeader uh, T arg);
        void Visit(AcceptHeader uh, T arg);
        void Visit(ContentTypeHeader uh, T arg);
        void Visit(AcceptCharsetHeader uh, T arg);
    }
}
