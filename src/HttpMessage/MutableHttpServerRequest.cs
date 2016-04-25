using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Restup.HttpMessage.Headers.Request;
using Restup.HttpMessage.Models.Contracts;
using Restup.HttpMessage.Models.Schemas;
using Restup.HttpMessage.ServerRequestParsers;

namespace Restup.HttpMessage
{
    public class MutableHttpServerRequest : IHttpServerRequest
    {
        private readonly List<IHttpRequestHeader> _headers;

        internal MutableHttpServerRequest()
        {
            _headers = new List<IHttpRequestHeader>();

            AcceptCharsets = Enumerable.Empty<string>();
            AcceptMediaTypes = Enumerable.Empty<string>();
            AcceptEncodings = Enumerable.Empty<string>();
        }

        public IEnumerable<IHttpRequestHeader> Headers => _headers;
        public HttpMethod? Method { get; set; }
        public Uri Uri { get; set; }
        public string HttpVersion { get; set; }
        public string ContentTypeCharset { get; set; }
        public IEnumerable<string> AcceptCharsets { get; set; }
        public int ContentLength { get; set; }
        public string ContentType { get; set; }
        public IEnumerable<string> AcceptEncodings { get; set; }
        public IEnumerable<string> AcceptMediaTypes { get; set; }
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

        public async static Task<MutableHttpServerRequest> Parse(IInputStream requestStream)
        {
            return await HttpRequestParser.Default.ParseRequestStream(requestStream);
        }
    }
}
