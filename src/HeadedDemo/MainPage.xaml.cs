using Devkoes.Restup.DemoControllers;
using Devkoes.Restup.WebServer;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HeadedDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private RestWebServer _webserver;

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
            _webserver = new RestWebServer(8800, "api");

            _webserver.RegisterController<AsyncControllerSample>();
            _webserver.RegisterController<FromContentControllerSample>();
            _webserver.RegisterController<PerCallControllerSample>();
            _webserver.RegisterController<SimpleParameterControllerSample>();
            _webserver.RegisterController<SingletonControllerSample>();
            _webserver.RegisterController<ThrowExceptionControllerSample>();
            _webserver.RegisterController<WithResponseContentControllerSample>();
            _webserver.RegisterController<FileSample>();

            await _webserver.StartServerAsync();
        }
    }
}
