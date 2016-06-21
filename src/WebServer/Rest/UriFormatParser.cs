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

        public ParsedUri Parse(string uriFormatUri)
        {
            var match = splitRegex.Match(uriFormatUri);
            if (!match.Success)
                throw new Exception($"Could not parse {uriFormatUri}");

            var uriGroup = match.Groups["uri"];
            if (!uriGroup.Success)
                throw new Exception($"Could not Parse the uri part of {uriFormatUri}");

            var parameters = ParseParameterGroup(match.Groups["parameters"]);
            var fragment = match.Groups["fragment"].Success ? match.Groups["fragment"].Value : string.Empty;

            return new ParsedUri(uriGroup.Value, parameters, fragment);
        }

        private static IReadOnlyCollection<UriParameter> ParseParameterGroup(Group group)
        {
            if (group.Success)
                return new UriParameter[] {};

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

            return new UriParameter(splitParameter[0], splitParameter[1]);
        }
    }
}