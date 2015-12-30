namespace Devkoes.Restup.WebServer.Models.Contracts
{
    interface IBodyRestResponse : IRestResponse
    {
        object BodyData { get; }
    }
}
