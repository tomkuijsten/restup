using System;
using Devkoes.Restup.WebServer.Models.Contracts;
using Devkoes.Restup.WebServer.Attributes;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    [RestVerb(RestVerb.GET)]
    public struct GetResponse : IRestResponse
    {
        public enum GetResponseStatus : int {
            OK = 200,
            NotFound = 404
        };

        public GetResponseStatus Status { get; }
        public string Data { get; }

        public GetResponse(GetResponseStatus status, string data)
        {
            Status = status;
            Data = data;
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
