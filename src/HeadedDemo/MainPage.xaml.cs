using Restup.DemoControllers;
using Restup.Webserver.File;
using Restup.Webserver.Http;
using Restup.Webserver.Rest;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Restup.HeadedDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private HttpServer _httpServer;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await InitializeWebServer();
        }

        private async Task InitializeWebServer()
        {
            var restRouteHandler = new RestRouteHandler();

            restRouteHandler.RegisterController<AsyncControllerSample>();
            restRouteHandler.RegisterController<FromContentControllerSample>();
            restRouteHandler.RegisterController<PerCallControllerSample>();
            restRouteHandler.RegisterController<SimpleParameterControllerSample>();
            restRouteHandler.RegisterController<SingletonControllerSample>();
            restRouteHandler.RegisterController<ThrowExceptionControllerSample>();
            restRouteHandler.RegisterController<WithResponseContentControllerSample>();

            var configuration = new HttpServerConfiguration()
                .ListenOnPort(8800)
                .RegisterRoute("api", restRouteHandler)
                .RegisterRoute("api", restRouteHandler)
                .RegisterRoute(new StaticFileRouteHandler(@"Restup.DemoStaticFiles\Web"))
                .EnableCors(); // allow cors requests on all origins
              //.EnableCors(x => x.AddAllowedOrigin("http://specificserver:<listen-port>"));

            var httpServer = new HttpServer(configuration);
            _httpServer = httpServer;
            await httpServer.StartServerAsync();


            // Don't release deferral, otherwise app will stop
        }
    }
}
