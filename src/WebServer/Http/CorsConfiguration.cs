using System.Collections.Generic;

namespace Restup.Webserver.Http
{
    internal class CorsConfiguration : ICorsConfiguration
    {
        public List<string> AllowedOrigins { get; } = new List<string>();

        public ICorsConfiguration AddAllowedOrigin(string allowedOrigin)
        {
            AllowedOrigins.Add(allowedOrigin);
            return this;
        }
    }
}