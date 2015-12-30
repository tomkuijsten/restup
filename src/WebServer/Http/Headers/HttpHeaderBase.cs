using Devkoes.Restup.WebServer.Models.Contracts;

namespace Devkoes.Restup.WebServer.Http.Headers
{
    internal abstract class HttpHeaderBase : IHttpHeader
    {
        public string Name { get; set; }
        public string RawContent { get; set; }

        public HttpHeaderBase(string name, string value)
        {
            Name = name;
            RawContent = value;
        }

        public abstract void Visit<T>(IHttpHeaderVisitor<T> v, T arg);
    }
}
