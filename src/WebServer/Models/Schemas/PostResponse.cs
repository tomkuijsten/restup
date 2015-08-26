using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    [RestVerb(RestVerb.POST)]
    public struct PostResponse : IRestResponse
    {
        public enum PostResponseStatus : int
        {
            Created = 201,
            Conflict = 409,
            NotFound = 404
        };

        public PostResponseStatus Status { get; }
        public object Data { get; }
        public string LocationRedirect { get; set; }

        public PostResponse(PostResponseStatus status, string locationRedirectUri)
        {
            Status = status;
            LocationRedirect = locationRedirectUri;
            Data = null;
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
