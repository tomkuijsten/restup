namespace Restup.HttpMessage.Models.Contracts
{
    interface IHttpResponsePartParser
    {
        string ParseToString(HttpServerResponse response);
        byte[] ParseToBytes(HttpServerResponse response);
    }
}
