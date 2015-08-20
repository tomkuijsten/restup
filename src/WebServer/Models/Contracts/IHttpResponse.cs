using Devkoes.Restup.WebServer.Models.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devkoes.Restup.WebServer.Models.Contracts
{
    public interface IHttpResponse
    {
        string Body { get; }
        AcceptMediaType BodyType { get; }
        int StatusCode { get; }
    }
}
