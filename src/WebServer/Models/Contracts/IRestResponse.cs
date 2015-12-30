namespace Devkoes.Restup.WebServer.Models.Contracts
{
    interface IRestResponse
    {
        int StatusCode { get; }

        T Visit<P, T>(IRestResponseVisitor<P, T> visitor, P param);
    }
}
