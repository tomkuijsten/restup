namespace Restup.HttpMessage.Models.Contracts
{
    public interface IHttpRequestHeader : IHttpHeader
    {
        void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg);
    }
}
