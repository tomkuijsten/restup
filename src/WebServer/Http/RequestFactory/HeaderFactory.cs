using System;
using System.Collections.Generic;
using System.Linq;

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
            return new ContentTypeHeader(headerValue, ExtractQuantifiedHeader(headerValue));
        }

        private IHttpHeader CreateResponseContentType(string headerValue)
        {
            return new AcceptHeader(headerValue, ExtractQuantifiedHeaders(headerValue));
        }

        private IHttpHeader CreateResponseContentCharset(string headerValue)
        {
            return new AcceptCharsetHeader(headerValue, ExtractQuantifiedHeaders(headerValue));
        }

        internal IEnumerable<QuantifiedHeaderValue> ExtractQuantifiedHeaders(string value)
        {
            var headerValues = value.Split(',');
            foreach (var headerValue in headerValues)
            {
                yield return ExtractQuantifiedHeader(headerValue);
            }

        }
        internal QuantifiedHeaderValue ExtractQuantifiedHeader(string value)
        {
            string headerValue = null;
            var extractedQuantifiers = new Dictionary<string, string>();
            var quantifiers = value.Split(';');
            if (quantifiers.Length > 0)
            {
                headerValue = quantifiers[0].TrimWhitespaces();
            }
            if (quantifiers.Length > 1)
            {
                foreach (var quantifier in quantifiers.Skip(1))
                {
                    var parts = quantifier.Split('=');
                    if (parts.Length > 1)
                    {
                        string qKey = parts[0].TrimWhitespaces();
                        string qValue = parts[1].TrimWhitespaces();
                        extractedQuantifiers.Add(qKey, qValue);
                    }
                }
            }

            return new QuantifiedHeaderValue(headerValue, extractedQuantifiers);
        }
    }
}
