using Devkoes.Restup.WebServer.Models.Schemas;

namespace Devkoes.Restup.WebServer.Models.Contracts
{
    public interface IRestResponse
    {
        int StatusCode { get; }

        void Accept(IRestResponseVisitor visitor);
    }

    public interface IBodyRestResponse : IRestResponse
    {
        object BodyData { get; }
    }
}
