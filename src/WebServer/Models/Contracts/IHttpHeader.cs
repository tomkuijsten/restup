namespace Devkoes.Restup.WebServer.Models.Contracts
{
    interface IHttpHeader
    {
        string Name { get; set; }
        string RawContent { get; set; }

        void Visit<T>(IHttpHeaderVisitor<T> v, T arg);
    }
}
