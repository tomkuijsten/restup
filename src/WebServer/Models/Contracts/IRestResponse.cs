namespace Devkoes.Restup.WebServer.Models.Contracts
{
    public interface IRestResponse
    {
        int StatusCode { get; }

        T Visit<P, T>(IRestResponseVisitor<P, T> visitor, P param);
    }

    public interface IBodyRestResponse : IRestResponse
    {
        object BodyData { get; }
    }
}
