namespace Devkoes.Restup.WebServer.Models.Contracts
{
    public interface IRestResponse
    {
        int StatusCode { get; }

        T Visit<T>(IRestResponseVisitor<T> visitor);
    }

    public interface IBodyRestResponse : IRestResponse
    {
        object BodyData { get; }
    }
}
