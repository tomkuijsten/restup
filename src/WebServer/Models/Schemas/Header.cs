
using Restup.Webserver.Models.Contracts;

namespace Restup.Webserver.Models.Schemas
{
    class Header : IHeader
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public Header( string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
