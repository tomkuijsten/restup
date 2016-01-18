namespace Devkoes.HttpMessage.Models.Contracts
{
    public interface IHttpHeader
    {
        string Name { get; set; }
        string RawContent { get; set; }

        void Visit<T>(IHttpHeaderVisitor<T> v, T arg);
    }
}
