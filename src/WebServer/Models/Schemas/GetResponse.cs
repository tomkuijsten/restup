using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Contracts;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    [RestVerb(HttpMethod.GET)]
    public struct GetResponse : IContentRestResponse
    {
        public enum ResponseStatus : int
        {
            OK = 200,
            NotFound = 404
        };

        public ResponseStatus Status { get; }
        public object ContentData { get; }

        public GetResponse(ResponseStatus status) : this(status, null) { }

        public GetResponse(ResponseStatus status, object data)
        {
            Status = status;
            ContentData = data;
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
