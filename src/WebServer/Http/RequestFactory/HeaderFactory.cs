using Devkoes.Restup.WebServer.Models.Schemas;
using System;
using System.Collections.Generic;

namespace Devkoes.Restup.WebServer.Http.RequestFactory
{
    internal class HeaderFactory
    {
        private Dictionary<string, Func<string, IHttpHeader>> _headerCollection;

        internal HeaderFactory()
        {
            _headerCollection = new Dictionary<string, Func<string, IHttpHeader>>()
            {
                [ContentLengthHeader.NAME] = CreateContentLength,
                [AcceptHeader.NAME] = CreateResponseContentType,
                [ContentTypeHeader.NAME] = CreateRequestContentType,
                [AcceptCharsetHeader.NAME] = CreateResponseContentCharset
            };
        }

        internal IHttpHeader Create(string headerName, string headerValue)
        {
            if (_headerCollection.ContainsKey(headerName))
            {
                return _headerCollection[headerName](headerValue);
            }

            return new UntypedHeader(headerName, headerValue);
        }

        private IHttpHeader CreateContentLength(string headerValue)
        {
            return new ContentLengthHeader(headerValue);
        }

        private IHttpHeader CreateRequestContentType(string headerValue)
        {
            return new ContentTypeHeader(headerValue);
        }

        private IHttpHeader CreateResponseContentType(string headerValue)
        {
            return new AcceptHeader(headerValue);
        }

        private IHttpHeader CreateResponseContentCharset(string headerValue)
        {
            return new AcceptCharsetHeader(headerValue);
        }
    }
}
