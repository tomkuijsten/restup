namespace WebServer.Rest.Models.Contracts
{
    public interface IContentRestResponse : IRestResponse
    {
        object ContentData { get; }
    }
}
