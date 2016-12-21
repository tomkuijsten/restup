﻿using Restup.DemoControllers;
using Restup.DemoControllers.Authentication;
using Restup.Webserver.File;
using Restup.Webserver.Http;
using Restup.Webserver.Rest;
using Restup.WebServer.Http;
using Windows.ApplicationModel.Background;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace Restup.HeadlessDemo
{
    public sealed class StartupTask : IBackgroundTask
    {
        private HttpServer _httpServer;

        private BackgroundTaskDeferral _deferral;

        /// <remarks>
        /// If you start any asynchronous methods here, prevent the task
        /// from closing prematurely by using BackgroundTaskDeferral as
        /// described in http://aka.ms/backgroundtaskdeferral
        /// </remarks>
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // This deferral should have an instance reference, if it doesn't... the GC will
            // come some day, see that this method is not active anymore and the local variable
            // should be removed. Which results in the application being closed.
            _deferral = taskInstance.GetDeferral();
			var authProvider = new BasicAuthorizationProvider("Please login", new DemoCredentialValidator());
			var restRouteHandler = new RestRouteHandler(authProvider);

			restRouteHandler.RegisterController<AsyncControllerSample>();
            restRouteHandler.RegisterController<FromContentControllerSample>();
			restRouteHandler.RegisterController<AuthenticatedPerCallControllerSample>();
			restRouteHandler.RegisterController<PerCallControllerSample>();
            restRouteHandler.RegisterController<SimpleParameterControllerSample>();
            restRouteHandler.RegisterController<SingletonControllerSample>();
            restRouteHandler.RegisterController<ThrowExceptionControllerSample>();
            restRouteHandler.RegisterController<WithResponseContentControllerSample>();

			var configuration = new HttpServerConfiguration()
				.ListenOnPort(8800)
				.RegisterRoute("api", restRouteHandler)
				.RegisterRoute(new StaticFileRouteHandler(@"Restup.DemoStaticFiles\Web", authProvider))
				.EnableCors(); // allow cors requests on all origins
            //  .EnableCors(x => x.AddAllowedOrigin("http://specificserver:<listen-port>"));

            var httpServer = new HttpServer(configuration);
            _httpServer = httpServer;
            
            await httpServer.StartServerAsync();

            // Dont release deferral, otherwise app will stop
        }
    }
}
