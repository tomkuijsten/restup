using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Restup.Webserver.Rest
{
    internal class UriParser
    {
        private readonly Regex splitRegex;

        public UriParser()
        {
            splitRegex = new Regex(@"(?<uri>.*?)(\?(?<parameters>.*?))?(#(?<fragment>.*?))?$");
        }

        public bool TryParse(string uriFormatUri, out ParsedUri parsedUri)
        {
            var match = splitRegex.Match(uriFormatUri);
            if (!match.Success)
            {
                parsedUri = null;
                return false;
            }

            var uriGroup = match.Groups["uri"];
            if (!uriGroup.Success)
            {
                parsedUri = null;
                return false;
            }

            var pathParts = ParsePathParts(uriGroup.Value);

            var parameters = ParseParameterGroup(match.Groups["parameters"]);
            var fragment = match.Groups["fragment"].Success ? match.Groups["fragment"].Value : string.Empty;

            parsedUri = new ParsedUri(pathParts, parameters, fragment);
            return true;
        }

        private IReadOnlyList<PathPart> ParsePathParts(string path)
        {
            return path.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Select(GetPathPart).ToArray();
        }

        private PathPart GetPathPart(string pathPart)
        {            
            if (pathPart.StartsWith("{") && pathPart.EndsWith("}"))
                return new PathPart(PathPart.PathPartType.Argument, pathPart.Substring(1, pathPart.Length - 2));

            return new PathPart(PathPart.PathPartType.Path, pathPart);
        }

        private static IReadOnlyList<UriParameter> ParseParameterGroup(Group group)
        {
            if (!group.Success)
                return new UriParameter[] { };

            var parameters = group.Value;
            var splitByAmpersand = parameters.Split('&');
            return splitByAmpersand.Select(ParseParameterPart).ToArray();
        }

        private static UriParameter ParseParameterPart(string parameter)
        {
            var splitParameter = parameter.Split('=');
            if (splitParameter.Length > 2)
                throw new Exception($"Could not parse parameter: {parameter}");

            if (splitParameter.Length == 1)
                return new UriParameter(splitParameter[0]);

            return new UriParameter(splitParameter[0], StripBraces(splitParameter[1]));
        }

        private static string StripBraces(string value)
        {
            if (value.StartsWith("{") && value.EndsWith("}"))
                return value.Substring(1, value.Length - 2);

            return value;
        }
    }
}