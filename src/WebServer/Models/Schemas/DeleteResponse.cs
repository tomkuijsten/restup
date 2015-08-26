using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    [RestVerb(RestVerb.DELETE)]
    public struct DeleteResponse : IRestResponse
    {
        public enum DeleteResponseStatus : int
        {
            OK = 200,
            NoContent = 204,
            NotFound = 404
        };

        public DeleteResponseStatus Status { get; }
        public object Data { get; }

        public DeleteResponse(DeleteResponseStatus status, object data = null)
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
