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

        public virtual T Visit<P, T>(IRestResponseVisitor<P, T> visitor, P param)
        {
            return visitor.Visit(this, param);
        }
    }
}
