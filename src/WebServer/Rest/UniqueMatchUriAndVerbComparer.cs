using System.Collections.Generic;
using System.Linq;

namespace Restup.Webserver.Rest
{
    internal class UniqueMatchUriAndVerbRestControllerMethodInfoComparer : IEqualityComparer<RestControllerMethodInfo>
    {
        private readonly PathPartsAndParametersParsedUriComparer _parsedUriComparer;

        public UniqueMatchUriAndVerbRestControllerMethodInfoComparer()
        {
            _parsedUriComparer = new PathPartsAndParametersParsedUriComparer();
        }

        public bool Equals(RestControllerMethodInfo x, RestControllerMethodInfo y)
        {
            return _parsedUriComparer.Equals(x.MatchUri, y.MatchUri) && x.Verb == y.Verb;
        }

        public int GetHashCode(RestControllerMethodInfo obj)
        {
            unchecked
            {
                return ((obj.MatchUri?.GetHashCode() ?? 0) * 397) ^ (int)obj.Verb;
            }
        }

        internal class PathPartsAndParametersParsedUriComparer : IEqualityComparer<ParsedUri>
        {
            public bool Equals(ParsedUri x, ParsedUri y)
            {
                return x.PathParts.SequenceEqual(y.PathParts) && x.Parameters.SequenceEqual(y.Parameters);
            }

            public int GetHashCode(ParsedUri obj)
            {
                unchecked
                {
                    return ((obj.Parameters?.GetHashCode() ?? 0) * 397) ^ (obj.PathParts?.GetHashCode() ?? 0);
                }
            }
        }
    }
}