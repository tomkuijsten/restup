using Devkoes.Restup.WebServer.Http.Headers;
using Devkoes.Restup.WebServer.Models.Contracts;
using Devkoes.Restup.WebServer.Models.Schemas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Devkoes.Restup.WebServer.Http
{
    internal class HttpRequest
    {
        private List<IHttpHeader> _headers;

        internal HttpRequest()
        {
            _headers = new List<IHttpHeader>();

            Method = HttpMethod.Unsupported;
            RequestContentEncoding = Constants.DefaultHttpMessageCharset;
            ResponseContentEncoding = Constants.DefaultHttpMessageCharset;
            RequestContentType = MediaType.JSON;
            ResponseContentType = MediaType.JSON;
        }

        internal IEnumerable<IHttpHeader> Headers => _headers;
        internal HttpMethod Method { get; set; }
        internal Uri Uri { get; set; }
        internal string HttpVersion { get; set; }
        internal Encoding RequestContentEncoding { get; set; }
        internal Encoding ResponseContentEncoding { get; set; }
        internal int ContentLength { get; set; }
        internal MediaType RequestContentType { get; set; }
        internal MediaType ResponseContentType { get; set; }
        internal string Content { get; set; }
        internal bool IsComplete { get; set; }

        internal void AddHeader(IHttpHeader header)
        {
            if (IsComplete)
            {
                throw new InvalidOperationException("Can't add header after processing request is finished!");
            }

            header.Visit(HttpRequestHandleHeaderData.Default, this);

            _headers.Add(header);
        }
    }
}
