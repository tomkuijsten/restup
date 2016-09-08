using System;
using System.Collections.Generic;
using Restup.Webserver.Models.Contracts;

namespace Restup.Webserver.Http
{
    public class HttpServerConfiguration
    {
        public int ServerPort { get; }
        internal CorsConfiguration CorsConfiguration { get; private set; }
        internal SortedSet<RouteRegistration> Routes { get; } = new SortedSet<RouteRegistration>();

        public HttpServerConfiguration(int serverPort)
        {
            ServerPort = serverPort;
        }

        /// <summary>
        /// Enables cors support on all origins (*).
        /// In the preflight request the cors headers have the following values:
        /// Access-Control-Allow-Methods = GET, POST, PUT, DELETE, OPTIONS
        /// Access-Control-Max-Age = 10 min
        /// Access-Control-Allow-Headers = mirrors the Access-Control-Request-Headers field of the request.
        /// </summary>
        public void EnableCors()
        {
            EnableCors(x => x.AddAllowedOrigin("*"));
        }

        /// <summary>
        /// Enables cors support and allows to specify the cors configuration.
        /// In the preflight request the cors headers have the following values:
        /// Access-Control-Allow-Methods = GET, POST, PUT, DELETE, OPTIONS
        /// Access-Control-Max-Age = 10 min
        /// Access-Control-Allow-Headers = mirrors the Access-Control-Request-Headers field of the request.
        /// </summary>
        /// <example>
        /// configuration.EnableCors(x => x
        ///                  .AddAllowedOrigin("http://server1.com")
        ///                  .AddAllowedOrigin("http://server2.com"));
        /// </example>
        /// <param name="builderFunc">The cors configuration builder function.</param>
        public void EnableCors(Action<ICorsConfiguration> builderFunc)
        {
            var corsConfiguration = new CorsConfiguration();
            builderFunc(corsConfiguration);
            CorsConfiguration = corsConfiguration;
        }
    }
}