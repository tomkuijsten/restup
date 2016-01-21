using Devkoes.HttpMessage.Models.Schemas;

namespace Devkoes.Restup.WebServer
{
    internal class Configuration
    {
        internal static Configuration Default { get; }

        static Configuration()
        {
            Default = new Configuration();
        }

        public Configuration()
        {
            AcceptType = MediaType.JSON;
            ContentType = MediaType.JSON;
        }

        public MediaType AcceptType { get; set; }
        public MediaType ContentType { get; set; }
    }
}
