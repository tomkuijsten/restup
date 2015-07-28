namespace Devkoes.Restup.WebServer.Models.Schemas
{
    internal class RestRequest
    {
        internal RestVerb Verb { get; set; }
        internal AcceptHeaders AcceptHeader { get; set; }
        internal string Body { get; set; }
        internal string Uri { get; set; }
    }
}
