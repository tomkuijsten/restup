using System;
using System.Collections.Generic;
using System.Linq;
using Restup.Webserver.Models.Contracts;

namespace Restup.Webserver.Http
{
    public class HttpServerConfiguration
    {
        public int ServerPort { get; private set; } = 80;
        internal CorsConfiguration CorsConfiguration { get; private set; }
        internal IEnumerable<RouteRegistration> Routes { get; private set; } = new RouteRegistration[] { };

        public HttpServerConfiguration ListenOnPort(int serverPort)
        {
            ServerPort = serverPort;
            return this;
        }

        /// <summary>
        /// Enables cors support on all origins (*).
        /// In the preflight request the cors headers have the following values:
        /// Access-Control-Allow-Methods = GET, POST, PUT, DELETE, OPTIONS
        /// Access-Control-Max-Age = 10 min
        /// Access-Control-Allow-Headers = mirrors the Access-Control-Request-Headers field of the request.
        /// </summary>
        public HttpServerConfiguration EnableCors()
        {
            return EnableCors(x => x.AddAllowedOrigin("*"));
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
        public HttpServerConfiguration EnableCors(Action<ICorsConfiguration> builderFunc)
        {
            var corsConfiguration = new CorsConfiguration();
            builderFunc(corsConfiguration);

            CorsConfiguration = corsConfiguration;
            return this;
        }

        /// <summary>
        /// Registers the <see cref="IRouteHandler"/> on the root url.
        /// </summary>
        /// <param name="restRoutehandler">The rest route handler to register.</param>
        public HttpServerConfiguration RegisterRoute(IRouteHandler restRoutehandler)
        {
            return RegisterRoute("/", restRoutehandler);
        }

        /// <summary>
        /// Registers the <see cref="IRouteHandler"/> on the specified url prefix.
        /// </summary>
        /// <param name="urlPrefix">The urlprefix to use, e.g. /api, /api/v001, etc. </param>
        /// <param name="restRoutehandler">The rest route handler to register.</param>
        public HttpServerConfiguration RegisterRoute(string urlPrefix, IRouteHandler restRoutehandler)
        {
            var routeRegistration = new RouteRegistration(urlPrefix, restRoutehandler);

            if (Routes.Contains(routeRegistration))
            {
                throw new Exception($"RouteHandler already registered for prefix: {urlPrefix}");
            }

            Routes = Routes.Concat(new[] { routeRegistration });
            return this;
        }
    }
}