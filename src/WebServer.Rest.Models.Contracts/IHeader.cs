using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devkoes.Restup.WebServer.Rest.Models.Contracts
{
    public interface IHeader
    {
        String Name { get; set; }
        String Value { get; set; }
    }
}
