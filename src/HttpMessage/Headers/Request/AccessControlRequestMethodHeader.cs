using Restup.HttpMessage.Models.Contracts;
using Restup.HttpMessage.Models.Schemas;
using Restup.HttpMessage.ServerRequestParsers;

namespace Restup.HttpMessage.Headers.Request
{
    public class AccessControlRequestMethodHeader : HttpRequestHeaderBase
    {
        internal static string NAME = "Access-Control-Request-Method";

        public HttpMethod Method { get; }

        public AccessControlRequestMethodHeader(string value) : base(NAME, value)
        {
            Method = HttpMethodParser.GetMethod(value);
        }

        public override void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg)
        {
            v.Visit(this, arg);
        }
    }
}