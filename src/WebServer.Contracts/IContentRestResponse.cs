namespace Devkoes.Restup.WebServer.Rest.Models.Contracts
{
    public interface IContentRestResponse : IRestResponse
    {
        object ContentData { get; }
    }
}
