using Restup.HttpMessage.Headers;
using Restup.HttpMessage.Headers.Request;

namespace Restup.HttpMessage.Models.Contracts
{
    public interface IHttpRequestHeaderVisitor<T>
    {
        void Visit(UntypedRequestHeader uh, T arg);
        void Visit(ContentLengthHeader uh, T arg);
        void Visit(AcceptHeader uh, T arg);
        void Visit(ContentTypeHeader uh, T arg);
        void Visit(AcceptCharsetHeader uh, T arg);
        void Visit(AcceptEncodingHeader uh, T arg);
    }
}
