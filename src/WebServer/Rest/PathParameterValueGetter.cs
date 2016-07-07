using System;

namespace Restup.Webserver.Rest
{
    internal class PathParameterValueGetter : ParameterValueGetter
    {
        private readonly int _pathIndex;

        public PathParameterValueGetter(string methodName, Type parameterType, int pathIndex) : base(methodName, parameterType)
        {
            _pathIndex = pathIndex;
        }

        protected override string GetValueFromUri(ParsedUri parsedUri)
        {
            return parsedUri.PathParts[_pathIndex].Value;
        }
    }
}