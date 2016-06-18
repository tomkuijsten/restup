
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devkoes.Restup.WebServer.Rest.Models.Contracts;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    class Header : IHeader
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public Header( String name, String value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
