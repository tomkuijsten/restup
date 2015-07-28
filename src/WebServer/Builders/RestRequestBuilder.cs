using Devkoes.Restup.WebServer.Helpers;
using Devkoes.Restup.WebServer.Models.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Devkoes.Restup.WebServer.Builders
{
    public class RestRequestBuilder
    {
        private const string ACCEPT_HEADERKEY = "accept:";

        // The order represents the priority for a response
        private static readonly IDictionary<AcceptHeaders, string> _acceptHeaderText = new Dictionary<AcceptHeaders, string>()
        {
            [AcceptHeaders.ApplicationJSON] = "application/json",
            [AcceptHeaders.TextXML] = "text/xml",
            [AcceptHeaders.TextHTML] = "text/html"
        };

        internal RestRequest Build(string request)
        {
            if (string.IsNullOrWhiteSpace(request))
            {
                throw new ArgumentException("Request should contain the http verb and uri", "request");
            }

            string[] perLine = request.Split('\n');
            
            var verbAndUri = GetVerbAndUriFromRequest(perLine);
            var verb = HttpHelpers.GetVerb(verbAndUri.Item1);
            string body = GetBodyFromRequest(request);
            var accHeader = GetAcceptHeaderFromRequest(perLine);

            return new RestRequest()
            {
                Verb = verb,
                Uri = verbAndUri.Item2,
                Body = body,
                AcceptHeader = accHeader
            };
        }

        private static Tuple<string,string> GetVerbAndUriFromRequest(string[] perLine)
        {
            string[] requestParts = perLine[0].Split(' ');

            if (requestParts.Length < 2)
            {
                throw new ArgumentException("Request should contain the http verb and uri", "request");
            }

            return Tuple.Create(requestParts[0], requestParts[1]);
        }

        private AcceptHeaders GetAcceptHeaderFromRequest(string[] requestLines)
        {
            // HTTP 1.1 headers are case insensitive (http://www.w3.org/Protocols/rfc2616/rfc2616.html)
            var lcLines = requestLines.Select(l => l.ToLower());
            var acceptedHeaders = 
                    from line in lcLines
                    where line.StartsWith(ACCEPT_HEADERKEY)
                    from header in _acceptHeaderText
                    where line.Contains(header.Value)
                    select header.Key;

            return acceptedHeaders.FirstOrDefault();
        }

        private string GetBodyFromRequest(string request)
        {
            string body = null;
            var bodyIndex = request.IndexOf("\r\n\r\n");
            if (bodyIndex != -1 && request.Length > (bodyIndex + 5))
            {
                body = request.Substring(bodyIndex + 4);
            }

            return body;
        }
    }
}
