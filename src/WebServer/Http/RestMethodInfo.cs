using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Contracts;
using Devkoes.Restup.WebServer.Models.Schemas;
using System.Reflection;
using System.Text.RegularExpressions;
using System;
using Devkoes.Restup.WebServer.InstanceCreators;
using System.Collections.Generic;
using System.Linq;

namespace Devkoes.Restup.WebServer.Http
{
    public class RestMethodInfo
    {
        private static readonly Regex FIND_PARAMETERKEYS_REGEX = new Regex("{(.*?)}");
        private const string MATCHPARAMETER_REPLACE_STRING = "(?<$1>.*?)";
        private const string MATCHURI_REPLACE_STRING = ".+?";

        private Regex _findParameterValuesRegex;
        private Regex _matchUriRegex;
        private IDictionary<string, Type> _parametersForUri;

        public MethodInfo MethodInfo { get; private set; }
        public RestVerb Verb { get; private set; }
        public bool HasBodyParameter{get; private set;}
        public Type BodyParameterType { get; private set;}

        public RestMethodInfo(MethodInfo methodInfo)
        {
            MethodInfo = methodInfo;

            InitializeParameterNames();
            InitializeVerb();
            InitializeFindParameterRegex();
            InitializeMatchUriRegex();
            InitializeBodyParameter();
        }

        private void InitializeBodyParameter()
        {
            var fromBodyParameter = MethodInfo.GetParameters().FirstOrDefault((p) => p.GetCustomAttribute<FromBodyAttribute>() != null);
            if (fromBodyParameter == null)
            {
                return;
            }

            HasBodyParameter = true;
            BodyParameterType = fromBodyParameter.ParameterType;
        }

        private void InitializeParameterNames()
        {
            var fromUriParams = from p in MethodInfo.GetParameters()
                    where p.GetCustomAttribute<FromBodyAttribute>() == null
                    select p;

            _parametersForUri = fromUriParams.ToDictionary(p => p.Name, p => p.ParameterType);
        }

        private void InitializeMatchUriRegex()
        {
            var uriFormatter = MethodInfo.GetCustomAttribute<UriFormatAttribute>();
            string regexToMatchUri = string.Format("^{0}$", FIND_PARAMETERKEYS_REGEX.Replace(uriFormatter.UriFormat, MATCHURI_REPLACE_STRING));
            _matchUriRegex = new Regex(regexToMatchUri);

        }

        private void InitializeFindParameterRegex()
        {
            var uriFormatter = MethodInfo.GetCustomAttribute<UriFormatAttribute>();

            string regexToFindParamValues = string.Format("^{0}$", FIND_PARAMETERKEYS_REGEX.Replace(uriFormatter.UriFormat, MATCHPARAMETER_REPLACE_STRING));

            _findParameterValuesRegex = new Regex(regexToFindParamValues);
        }

        private void InitializeVerb()
        {
            var restVerbAttr = MethodInfo.ReturnType.GetTypeInfo().GetCustomAttribute<RestVerbAttribute>();
            Verb = restVerbAttr.Verb;
        }

        public bool Match(RestVerb verb, string uri)
        {
            return this.Verb == verb && UriMatches(uri);
        }

        private bool UriMatches(string uri)
        {
            return _matchUriRegex.IsMatch(uri);
        }

        public IEnumerable<object> GetParametersFromUri(string uri)
        {
            Match m = _findParameterValuesRegex.Match(uri);
            if(!m.Success)
            {
                yield return null;
            }

            foreach (var parameter in _parametersForUri)
            {
                yield return Convert.ChangeType(m.Groups[parameter.Key].Value, parameter.Value);
            }
        }
    }
}
