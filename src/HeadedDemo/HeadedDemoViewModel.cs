using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Restup.HeadedDemo
{
    public class HeadedDemoViewModel : INotifyPropertyChanged
    {
        private readonly Uri _baseUri;

        private bool isNotSendingRequest;
        private string _output;

        private string _relativeUri;
        private string _method;
        private string _body;
        private Uri _webViewUri;

        public bool IsNotSendingRequest
        {
            get { return isNotSendingRequest; }
            private set
            {
                isNotSendingRequest = value;
                OnPropertyChanged();
                IsBodyAcceptingInput = true;
            }
        }

        public bool IsBodyAcceptingInput
        {
            get { return IsNotSendingRequest && Method != "GET"; }
            private set { OnPropertyChanged(); } // just to notify that the property has changed
        }

        public string Output
        {
            get { return _output; }
            private set { _output = value; OnPropertyChanged(); }
        }

        public string RelativeUri
        {
            get { return _relativeUri; }
            set { _relativeUri = value; OnPropertyChanged(); }
        }

        public Uri WebViewUri
        {
            get { return _webViewUri; }
            set { _webViewUri = value; OnPropertyChanged(); }
        }

        public string Method
        {
            get { return _method; }
            set
            {
                _method = value;
                OnPropertyChanged();
                IsBodyAcceptingInput = true;
            }
        }

        public string Body
        {
            get { return _body; }
            set { _body = value; OnPropertyChanged(); }
        }

        public ICommand SendCommand { get; private set; }

        public HeadedDemoViewModel()
        {
            SendCommand = new DelegateCommand<string>(SendRequest, _ => IsNotSendingRequest);

            _baseUri = new Uri("http://127.0.0.1:8800/");
            Method = "GET";
            RelativeUri = "/api/simpleparameter/2/property/propname";
            WebViewUri = GetAbsoluteUri();
            IsNotSendingRequest = true;
        }

        private async void SendRequest(string body)
        {
            IsNotSendingRequest = false;

            try
            {
                await SendRequest();
            }
            catch (Exception ex)
            {
                Output = ex.ToString();
            }
            finally
            {
                IsNotSendingRequest = true;
            }
        }

        private async Task SendRequest()
        {
            var requestUri = GetAbsoluteUri();
            WebViewUri = requestUri;

            var webRequest = WebRequest.CreateHttp(requestUri);
            webRequest.Accept = "application/json";
            webRequest.Method = Method;

            if (webRequest.Method != "GET" && !string.IsNullOrWhiteSpace(Body))
            {
                webRequest.ContentType = "application/json";

                var requestStream = await webRequest.GetRequestStreamAsync();
                using (var streamWriter = new StreamWriter(requestStream))
                {
                    await streamWriter.WriteAsync(Body);
                }
            }

            var response = await webRequest.GetResponseAsync();
            var responseStream = response.GetResponseStream();

            using (var streamReader = new StreamReader(responseStream))
            {
                var readAll = await streamReader.ReadToEndAsync();
                WriteToOutput((HttpWebResponse)response, readAll);
            }
            response.Dispose();
        }

        private Uri GetAbsoluteUri()
        {
            return new Uri(_baseUri + RelativeUri.TrimStart('/'));
        }

        private void WriteToOutput(HttpWebResponse response, string readAll)
        {
            var outputStringBuilder = new StringBuilder();
            outputStringBuilder.AppendLine(response.ResponseUri.ToString());
            outputStringBuilder.AppendFormat("{0} {1}", ((int)response.StatusCode), response.StatusDescription).AppendLine();

            foreach (var header in response.Headers)
            {
                outputStringBuilder.AppendFormat("{0}: {1}", header, response.Headers[header.ToString()]).AppendLine();
            }
            outputStringBuilder.AppendLine();
            outputStringBuilder.Append(readAll);

            Output = outputStringBuilder.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}