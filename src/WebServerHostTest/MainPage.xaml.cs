using Devkoes.Restup.WebServer;
using System.Threading.Tasks;
using WebServerHostTest.RESTControllers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WebServerHostTest
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
            _webserver = new RestWebServer();
            _webserver.RegisterController<ParametersController>();

            await _webserver.StartServerAsync();
        }
    }
}
