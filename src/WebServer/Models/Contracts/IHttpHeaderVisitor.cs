using Devkoes.Restup.WebServer.Http.Headers;

namespace Devkoes.Restup.WebServer.Models.Contracts
{
    interface IHttpHeaderVisitor<T>
    {
        void Visit(UntypedHeader uh, T arg);
        void Visit(ContentLengthHeader uh, T arg);
        void Visit(AcceptHeader uh, T arg);
        void Visit(ContentTypeHeader uh, T arg);
        void Visit(AcceptCharsetHeader uh, T arg);
    }
}
