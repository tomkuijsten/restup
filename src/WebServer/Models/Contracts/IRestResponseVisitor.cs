using Devkoes.Restup.WebServer.Models.Schemas;

namespace Devkoes.Restup.WebServer.Models.Contracts
{
    public interface IRestResponseVisitor<TParam, TResult>
    {
        TResult Visit(PutResponse response, TParam param);
        TResult Visit(GetResponse response, TParam param);
        TResult Visit(DeleteResponse response, TParam param);
        TResult Visit(PostResponse response, TParam param);
        TResult Visit(StatusOnlyResponse statusOnlyResponse, TParam param);
        TResult Visit(MethodNotAllowedResponse methodNotAllowedResponse, TParam param);
    }
}
