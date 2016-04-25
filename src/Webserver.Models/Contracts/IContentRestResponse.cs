namespace Restup.Webserver.Models.Contracts
{
    public interface IContentRestResponse : IRestResponse
    {
        object ContentData { get; }
    }
}
