using Devkoes.Restup.WebServer.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    public class StatusOnlyResponse : IRestResponse
    {
        public int StatusCode { get; private set; }

        public StatusOnlyResponse(int statusCode)
        {
            StatusCode = statusCode;
        }

        public virtual void Accept(IRestResponseVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
