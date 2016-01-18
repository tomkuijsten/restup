using Devkoes.HttpMessage.Headers.Request;
using Devkoes.HttpMessage.Models.Contracts;
using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.HttpMessage.Plumbing;
using Devkoes.HttpMessage.ServerRequestParsers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Devkoes.HttpMessage
{
    public class HttpServerRequest
    {
        private List<IHttpRequestHeader> _headers;

        internal HttpServerRequest()
        {
            _headers = new List<IHttpRequestHeader>();

            Method = HttpMethod.Unsupported;
            RequestContentEncoding = Constants.DefaultHttpEncoding;
            ResponseContentEncoding = Constants.DefaultHttpEncoding;
            RequestContentType = MediaType.JSON;
            ResponseContentType = MediaType.JSON;
        }

        public IEnumerable<IHttpRequestHeader> Headers => _headers;
        public HttpMethod Method { get; set; }
        public Uri Uri { get; set; }
        public string HttpVersion { get; set; }
        public Encoding RequestContentEncoding { get; set; }
        public Encoding ResponseContentEncoding { get; set; }
        public int ContentLength { get; set; }
        public MediaType RequestContentType { get; set; }
        public MediaType ResponseContentType { get; set; }
        public string Content { get; set; }
        public bool IsComplete { get; set; }

        internal void AddHeader(IHttpRequestHeader header)
        {
            if (IsComplete)
            {
                throw new InvalidOperationException("Can't add header after processing request is finished!");
            }

            header.Visit(HttpRequestHandleHeaderData.Default, this);

            _headers.Add(header);
        }

        public async static Task<HttpServerRequest> Parse(IInputStream requestStream)
        {
            return await HttpRequestParser.Default.ParseRequestStream(requestStream);
        }
    }
}
