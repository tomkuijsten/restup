using System.Collections.Generic;
using System.Linq;

namespace Restup.Webserver.Rest
{
    internal class ParsedUri
    {
        public string Path { get; set; }
        public IReadOnlyCollection<UriParameter> Parameters { get;}
        public string Fragment { get; set; }

        public ParsedUri(string path, IReadOnlyCollection<UriParameter> parameters, string fragment)
        {
            Path = path;
            Parameters = parameters;
            Fragment = fragment;
        }

        public override string ToString()
        {
            return $"Path={Path}, Parameters={string.Join("&", Parameters.Select(x => $"{x.Name}={x.Value}"))}, Fragment={Fragment}";
        }
    }
}