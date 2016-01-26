using Devkoes.Restup.DemoControllers;
using Devkoes.Restup.WebServer;
using Windows.ApplicationModel.Background;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace Devkoes.Restup.HeadlessDemo
{
    public sealed class StartupTask : IBackgroundTask
    {
        private RestWebServer _webserver;

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

            _webserver = new RestWebServer(8800, "api");

            _webserver.RegisterController<AsyncControllerSample>();
            _webserver.RegisterController<FromContentControllerSample>();
            _webserver.RegisterController<PerCallControllerSample>();
            _webserver.RegisterController<SimpleParameterControllerSample>();
            _webserver.RegisterController<SingletonControllerSample>();
            _webserver.RegisterController<ThrowExceptionControllerSample>();
            _webserver.RegisterController<WithResponseContentControllerSample>();

            await _webserver.StartServerAsync();

            // Dont release deferral, otherwise app will stop
        }
    }
}
