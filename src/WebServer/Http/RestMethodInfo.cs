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
        private IDictionary<string, Type> _parameters;

        public MethodInfo MethodInfo { get; set; }
        public RestVerb Verb { get; set; }
        public IInstanceCreator Instantiator { get; set; }

        public RestMethodInfo(MethodInfo methodInfo)
        {
            MethodInfo = methodInfo;

            InitializeParameterNames();
            InitializeInstantiator();
            InitializeVerb();
            InitializeFindParameterRegex();
            InitializeMatchUriRegex();
        }

        private void InitializeParameterNames()
        {
            _parameters = MethodInfo.GetParameters().ToDictionary(p=> p.Name, p=> p.ParameterType);
        }

        private void InitializeInstantiator()
        {
            Instantiator = InstanceCreatorCache.GetCreator(MethodInfo.DeclaringType);
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

            foreach (var parameter in _parameters)
            {
                yield return Convert.ChangeType(m.Groups[parameter.Key].Value, parameter.Value);
            }
        }

        public IRestResponse ExecuteMethod(string uri)
        {
            return (IRestResponse)MethodInfo.Invoke(
                    Instantiator.Create(MethodInfo.DeclaringType),
                    GetParametersFromUri(uri).ToArray());
        }
    }
}
