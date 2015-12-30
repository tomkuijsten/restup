using Devkoes.Restup.WebServer.Models.Contracts;

namespace Devkoes.Restup.WebServer.Http.Headers
{
    internal class UntypedHeader : HttpHeaderBase
    {
        public UntypedHeader(string name, string value) : base(name, value) { }

        public override void Visit<T>(IHttpHeaderVisitor<T> v, T arg)
        {
            v.Visit(this, arg);
        }
    }
}
