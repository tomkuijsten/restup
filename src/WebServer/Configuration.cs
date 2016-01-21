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
            ResponseContentType = MediaType.JSON;
        }

        public MediaType ResponseContentType { get; set; }
    }
}
