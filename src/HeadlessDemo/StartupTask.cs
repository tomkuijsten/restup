using Devkoes.Restup.DemoControllers;
using Devkoes.Restup.WebServer;
using Windows.ApplicationModel.Background;
using Devkoes.Restup.WebServer.Http;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace Devkoes.Restup.HeadlessDemo
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
            
            var httpServer = new HttpServer(8800);
            _httpServer = httpServer;

            var restRouteHandler = new RestRoutehandler();

            restRouteHandler.RegisterController<AsyncControllerSample>();
            restRouteHandler.RegisterController<FromContentControllerSample>();
            restRouteHandler.RegisterController<PerCallControllerSample>();
            restRouteHandler.RegisterController<SimpleParameterControllerSample>();
            restRouteHandler.RegisterController<SingletonControllerSample>();
            restRouteHandler.RegisterController<ThrowExceptionControllerSample>();
            restRouteHandler.RegisterController<WithResponseContentControllerSample>();

            httpServer.RegisterRoute("api", restRouteHandler);

            await httpServer.StartServerAsync();

            // Dont release deferral, otherwise app will stop
        }
    }
}
