using Devkoes.Restup.WebServer.Helpers;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Models.Contracts;
using Devkoes.Restup.WebServer.Models.Schemas;
using System.Linq;
using System.Text;
using System;

namespace Devkoes.Restup.WebServer.Visitors
{
    internal class RestResponseVisitor : IRestResponseVisitor
    {
        private RestRequest _request;
        private static BodySerializer _bodySerializer;

        internal IHttpResponse HttpResponse { get; private set; }

        static RestResponseVisitor()
        {
            _bodySerializer = new BodySerializer();
        }

        public RestResponseVisitor(RestRequest restRequest)
        {
            _request = restRequest;
        }

        public void Visit(DeleteResponse response)
        {
            var rawHttpResponseBuilder = new StringBuilder();
            rawHttpResponseBuilder.AppendFormat(CreateHttpStatusResponse(response));
            rawHttpResponseBuilder.Append("Connection: close\r\n");

            string completeResponse = rawHttpResponseBuilder.ToString();
            byte[] rawResponse = Encoding.UTF8.GetBytes(completeResponse);

            HttpResponse = new HttpResponse(completeResponse, rawResponse);
        }

        public void Visit(PostResponse response)
        {
            var rawHttpResponseBuilder = new StringBuilder();
            rawHttpResponseBuilder.AppendFormat(CreateHttpStatusResponse(response));

            if(response.Status == PostResponse.ResponseStatus.Created)
                rawHttpResponseBuilder.Append($"Location: {response.LocationRedirect}\r\n");

            rawHttpResponseBuilder.Append("Connection: close\r\n");

            string completeResponse = rawHttpResponseBuilder.ToString();
            byte[] rawResponse = Encoding.UTF8.GetBytes(completeResponse);

            HttpResponse = new HttpResponse(completeResponse, rawResponse);
        }

        public void Visit(GetResponse response)
        {
            string bodyString = _bodySerializer.ToBody(response.BodyData, _request);

            int bodyLength = bodyString == null ? 0 : Encoding.UTF8.GetBytes(bodyString).Length;

            var rawHttpResponseBuilder = new StringBuilder();
            rawHttpResponseBuilder.AppendFormat(CreateHttpStatusResponse(response));
            rawHttpResponseBuilder.AppendFormat("Content-Length: {0}\r\n", bodyLength);
            rawHttpResponseBuilder.AppendFormat("Content-Type: {0}\r\n", HttpHelpers.GetMediaType(_request.AcceptHeaders.First()));
            rawHttpResponseBuilder.Append("Connection: close\r\n");
            rawHttpResponseBuilder.Append("\r\n");
            rawHttpResponseBuilder.Append(bodyString);

            string completeResponse = rawHttpResponseBuilder.ToString();
            byte[] rawResponse = Encoding.UTF8.GetBytes(completeResponse);

            HttpResponse = new HttpResponse(completeResponse, rawResponse);
        }

        public void Visit(PutResponse response)
        {
            var rawHttpResponseBuilder = new StringBuilder();
            rawHttpResponseBuilder.AppendFormat(CreateHttpStatusResponse(response));
            rawHttpResponseBuilder.Append("Connection: close\r\n");

            string completeResponse = rawHttpResponseBuilder.ToString();
            byte[] rawResponse = Encoding.UTF8.GetBytes(completeResponse);

            HttpResponse = new HttpResponse(completeResponse, rawResponse);
        }

        private string CreateHttpStatusResponse(IRestResponse response)
        {
            string statusCodeText = HttpHelpers.GetHttpStatusCodeText(response.StatusCode);

            return $"HTTP/1.1 {response.StatusCode} {statusCodeText}\r\n";
        }

        public void Visit(StatusOnlyResponse statusOnlyResponse)
        {
            var rawHttpResponseBuilder = new StringBuilder();
            rawHttpResponseBuilder.AppendFormat(CreateHttpStatusResponse(statusOnlyResponse));
            rawHttpResponseBuilder.Append("Connection: close\r\n");

            string completeResponse = rawHttpResponseBuilder.ToString();
            byte[] rawResponse = Encoding.UTF8.GetBytes(completeResponse);

            HttpResponse = new HttpResponse(completeResponse, rawResponse);
        }

        public void Visit(MethodNotAllowedResponse methodNotAllowedResponse)
        {
            var rawHttpResponseBuilder = new StringBuilder();
            rawHttpResponseBuilder.AppendFormat(CreateHttpStatusResponse(methodNotAllowedResponse));
            rawHttpResponseBuilder.AppendFormat("Allow: {0}", string.Join(",", methodNotAllowedResponse.Allows));
            rawHttpResponseBuilder.Append("Connection: close\r\n");

            string completeResponse = rawHttpResponseBuilder.ToString();
            byte[] rawResponse = Encoding.UTF8.GetBytes(completeResponse);

            HttpResponse = new HttpResponse(completeResponse, rawResponse);
        }
    }
}
