using Devkoes.Restup.WebServer.Models.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devkoes.Restup.WebServer.Models.Contracts
{
    public interface IRestResponseVisitor
    {
        void Visit(PutResponse response);
        void Visit(GetResponse response);
        void Visit(DeleteResponse response);
        void Visit(PostResponse response);
        void Visit(DefaultResponse defaultResponse);
    }
}
