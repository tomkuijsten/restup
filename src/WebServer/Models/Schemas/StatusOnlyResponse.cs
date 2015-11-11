using Devkoes.Restup.WebServer.Models.Contracts;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    public class StatusOnlyResponse : IRestResponse
    {
        public int StatusCode { get; private set; }

        public StatusOnlyResponse(int statusCode)
        {
            StatusCode = statusCode;
        }

        public virtual T Visit<T>(IRestResponseVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
