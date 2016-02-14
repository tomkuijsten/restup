using System;
using System.Collections.Generic;
using Devkoes.HttpMessage.Models.Contracts;
using Devkoes.HttpMessage.Models.Schemas;

namespace Devkoes.HttpMessage
{
    internal class HttpServerRequest : IHttpServerRequest
    {
        public IEnumerable<IHttpRequestHeader> Headers { get; }
        public HttpMethod? Method { get; }
        public Uri Uri { get; }
        public string HttpVersion { get; }
        public string ContentTypeCharset { get; }
        public IEnumerable<string> AcceptCharsets { get; }
        public int ContentLength { get; }
        public MediaType? ContentType { get; }
        public IEnumerable<MediaType> AcceptMediaTypes { get; }
        public byte[] Content { get; }
        public bool IsComplete { get; }

        public HttpServerRequest(IEnumerable<IHttpRequestHeader> headers, HttpMethod? method, Uri uri,
            string httpVersion, string contentTypeCharset, IEnumerable<string> acceptCharsets, int contentLength,
            MediaType? contentType, IEnumerable<MediaType> acceptMediaTypes, byte[] content, bool isComplete)
        {
            Headers = headers;
            Method = method;
            Uri = uri;
            HttpVersion = httpVersion;
            ContentTypeCharset = contentTypeCharset;
            AcceptCharsets = acceptCharsets;
            ContentLength = contentLength;
            ContentType = contentType;
            AcceptMediaTypes = acceptMediaTypes;
            Content = content;
            IsComplete = isComplete;
        }
    }
}