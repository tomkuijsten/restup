namespace Devkoes.Restup.WebServer.Models.Contracts
{
    public interface IContentRestResponse : IRestResponse
    {
        object ContentData { get; }
    }
}
