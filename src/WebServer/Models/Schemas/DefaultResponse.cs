using Devkoes.Restup.WebServer.Models.Contracts;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    public class DefaultResponse : IRestResponse
    {
        public enum ResponseStatus : int
        {
            BadRequest = 400
        };

        public object Data { get; private set; }
        public int StatusCode { get; private set; }

        public DefaultResponse(string data, ResponseStatus code)
        {
            Data = data;
            StatusCode = (int)code;
        }

        public void Accept(IRestResponseVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
