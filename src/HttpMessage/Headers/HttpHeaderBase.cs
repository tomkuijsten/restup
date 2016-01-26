using Devkoes.HttpMessage.Models.Contracts;

namespace Devkoes.HttpMessage.Headers
{
    public abstract class HttpHeaderBase : IHttpHeader
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public HttpHeaderBase(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
