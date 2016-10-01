using System.Collections.Generic;
using System.Linq;

namespace Restup.Webserver.Rest
{
    internal class ParsedUri
    {
        public IReadOnlyList<PathPart> PathParts { get; }
        public IReadOnlyList<UriParameter> Parameters { get; }
        public string Fragment { get; }

        public ParsedUri(IReadOnlyList<PathPart> pathParts, IReadOnlyList<UriParameter> parameters, string fragment)
        {
            PathParts = pathParts;
            Parameters = parameters;
            Fragment = fragment;
        }

        public override string ToString()
        {
            return $"Path={string.Join("/", PathParts.Select(x => x.PartType == PathPart.PathPartType.Argument ? $"{{{x.Value}}}" : x.Value))}, Parameters={string.Join("&", Parameters.Select(x => $"{x.Name}={x.Value}"))}, Fragment={Fragment}";
        }
    }
}