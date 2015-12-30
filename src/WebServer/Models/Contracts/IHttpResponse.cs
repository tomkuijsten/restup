namespace Devkoes.Restup.WebServer.Models.Contracts
{
    interface IHttpResponse
    {
        string Response { get; }
        byte[] RawResponse { get; }
    }
}
