using Restup.HttpMessage.Models.Contracts;

namespace Restup.HttpMessage.Headers
{
    public abstract class HttpHeaderBase : IHttpHeader
    {
        public string Name { get; }
        public string Value { get; }

        protected HttpHeaderBase(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
