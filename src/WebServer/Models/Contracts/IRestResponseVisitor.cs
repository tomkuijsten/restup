using Devkoes.Restup.WebServer.Models.Schemas;

namespace Devkoes.Restup.WebServer.Models.Contracts
{
    public interface IRestResponseVisitor<T>
    {
        T Visit(PutResponse response);
        T Visit(GetResponse response);
        T Visit(DeleteResponse response);
        T Visit(PostResponse response);
        T Visit(StatusOnlyResponse statusOnlyResponse);
        T Visit(MethodNotAllowedResponse methodNotAllowedResponse);
    }
}
