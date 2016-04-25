using Restup.HttpMessage.Models.Contracts;
using Restup.HttpMessage.Plumbing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restup.HttpMessage.Headers.Request
{
    internal class RequestHeaderFactory
    {
        private Dictionary<string, Func<string, IHttpRequestHeader>> _headerCollection;

        internal RequestHeaderFactory()
        {
            _headerCollection = new Dictionary<string, Func<string, IHttpRequestHeader>>(StringComparer.OrdinalIgnoreCase)
            {
                [ContentLengthHeader.NAME] = CreateContentLength,
                [AcceptHeader.NAME] = CreateResponseContentType,
                [ContentTypeHeader.NAME] = CreateRequestContentType,
                [AcceptCharsetHeader.NAME] = CreateResponseContentCharset,
                [AcceptEncodingHeader.NAME] = CreateResponseAcceptEncoding
            };
        }

        internal IHttpRequestHeader Create(string headerName, string headerValue)
        {
            if (_headerCollection.ContainsKey(headerName))
            {
                return _headerCollection[headerName](headerValue);
            }

            return new UntypedRequestHeader(headerName, headerValue);
        }

        private IHttpRequestHeader CreateContentLength(string headerValue)
        {
            return new ContentLengthHeader(headerValue);
        }

        private IHttpRequestHeader CreateRequestContentType(string headerValue)
        {
            return new ContentTypeHeader(headerValue, ExtractQuantifiedHeader(headerValue));
        }

        private IHttpRequestHeader CreateResponseContentType(string headerValue)
        {
            return new AcceptHeader(headerValue, ExtractQuantifiedHeaders(headerValue));
        }

        private IHttpRequestHeader CreateResponseContentCharset(string headerValue)
        {
            return new AcceptCharsetHeader(headerValue, ExtractQuantifiedHeaders(headerValue));
        }

        private IHttpRequestHeader CreateResponseAcceptEncoding(string headerValue)
        {
            return new AcceptEncodingHeader(headerValue, ExtractQuantifiedHeaders(headerValue));
        }

        internal IEnumerable<QuantifiedHeaderValue> ExtractQuantifiedHeaders(string value)
        {
            var headerValues = value.Split(',');
            var quantifiedValues = headerValues.Select(h => ExtractQuantifiedHeader(h));

            return quantifiedValues.OrderByDescending(q => q.Quality).ToArray();
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
