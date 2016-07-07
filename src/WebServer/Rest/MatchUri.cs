using System.Collections.Generic;

namespace Restup.Webserver.Rest
{
    internal class MatchUri
    {
        public string Path { get; }
        public IReadOnlyCollection<string> PathParameters { get; }
        public IReadOnlyCollection<string> UriParameters { get; }

        public MatchUri(string path, IReadOnlyCollection<string> pathParameters, IReadOnlyCollection<string> uriParameters)
        {
            Path = path;
            PathParameters = pathParameters;
            UriParameters = uriParameters;
        }
    }
}