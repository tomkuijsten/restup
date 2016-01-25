namespace Devkoes.Restup.WebServer.Models.Contracts
{
    interface IContentRestResponse : IRestResponse
    {
        object ContentData { get; }
    }
}
