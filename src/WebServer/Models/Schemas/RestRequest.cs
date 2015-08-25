using System.Collections.Generic;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    internal class RestRequest
    {
        internal RestVerb Verb { get; set; }
        internal IEnumerable<MediaType> AcceptHeaders { get; set; }
        internal string Body { get; set; }
        internal MediaType BodyMediaType { get; set; }
        internal string Uri { get; set; }
    }
}
