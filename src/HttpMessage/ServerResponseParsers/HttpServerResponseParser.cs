using Restup.HttpMessage.Models.Contracts;
using System.Collections.Generic;
using System.Text;

namespace Restup.HttpMessage.ServerResponseParsers
{
    internal class HttpServerResponseParser
    {
        internal static HttpServerResponseParser Default { get; }

        static HttpServerResponseParser()
        {
            Default = new HttpServerResponseParser();
        }

        private IEnumerable<IHttpResponsePartParser> _pipeline;

        public HttpServerResponseParser()
        {
            _pipeline = new IHttpResponsePartParser[] {
                new StartLineParser(),
                new HeadersParser(),
                new ContentParser()
            };
        }

        public string ConvertToString(HttpServerResponse response)
        {
            var responseBuilder = new StringBuilder();
            foreach (var pipelinePart in _pipeline)
            {
                responseBuilder.Append(pipelinePart.ParseToString(response));
            }

            return responseBuilder.ToString();
        }

        public byte[] ConvertToBytes(HttpServerResponse response)
        {
            var responseBytes = new List<byte>();
            foreach (var pipelinePart in _pipeline)
            {
                responseBytes.AddRange(pipelinePart.ParseToBytes(response));
            }

            return responseBytes.ToArray();
        }
    }
}
