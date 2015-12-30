using Devkoes.Restup.WebServer;
using Devkoes.Restup.WebServerHostTest.RestControllers;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Devkoes.Restup.WebServerHostTest
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
            _webserver.RegisterController<FromBodyControllerSample>();
            _webserver.RegisterController<PerCallControllerSample>();
            _webserver.RegisterController<SimpleParameterControllerSample>();
            _webserver.RegisterController<SingletonControllerSample>();
            _webserver.RegisterController<ThrowExceptionControllerSample>();
            _webserver.RegisterController<WithResponseBodyControllerSample>();

            await _webserver.StartServerAsync();
        }
    }
}
