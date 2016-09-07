using System.Collections.Generic;

namespace Restup.Webserver.Http
{
    internal class CorsConfigurationBuilder : ICorsConfigurationBuilder
    {
        public List<string> AllowedOrigins { get; } = new List<string>();

        public ICorsConfigurationBuilder AddAllowedOrigin(string allowedOrigin)
        {
            AllowedOrigins.Add(allowedOrigin);
            return this;
        }
    }
}