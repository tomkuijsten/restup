using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Contracts;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    [RestVerb(HttpMethod.PUT)]
    public struct PutResponse : IContentRestResponse
    {
        public enum ResponseStatus : int
        {
            OK = 200,
            NoContent = 204,
            NotFound = 404
        };

        public ResponseStatus Status { get; }

        public object ContentData { get; set; }

        public PutResponse(ResponseStatus status, object content)
        {
            Status = status;
            ContentData = content;
        }

        public PutResponse(ResponseStatus status) : this(status, null)
        {
        }

        public static PutResponse CreateNotFound()
        {
            return new PutResponse(ResponseStatus.NotFound, null);
        }

        public int StatusCode
        {
            get
            {
                return (int)Status;
            }
        }
    }
}
