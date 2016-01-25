using Devkoes.HttpMessage.Headers.Request;
using Devkoes.HttpMessage.Models.Contracts;
using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.HttpMessage.Plumbing;
using Devkoes.HttpMessage.ServerRequestParsers;
using System;
using System.Collections.Generic;
using System.Linq;
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

            ContentTypeCharset = Constants.DefaultHttpCharset;

            AcceptCharsets = Enumerable.Empty<string>();
            AcceptMediaTypes = Enumerable.Empty<MediaType>();
        }

        public IEnumerable<IHttpRequestHeader> Headers => _headers;
        public HttpMethod? Method { get; set; }
        public Uri Uri { get; set; }
        public string HttpVersion { get; set; }
        public string ContentTypeCharset { get; set; }
        public IEnumerable<string> AcceptCharsets { get; set; }
        public int ContentLength { get; set; }
        public MediaType? ContentType { get; set; }
        public IEnumerable<MediaType> AcceptMediaTypes { get; set; }
        public byte[] Content { get; set; }
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
