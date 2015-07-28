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
        private IEnumerable<string> _parameterNames;

        public MethodInfo MethodInfo { get; set; }
        public RestVerb Verb { get; set; }
        public IInstanceCreator Instantiator { get; set; }

        public RestMethodInfo(MethodInfo methodInfo)
        {
            InitializeParameterNames(methodInfo);
            InitializeInstantiator(methodInfo);
            InitializeVerb(methodInfo);
            InitializeFindParameterRegex(methodInfo);
            InitializeMatchUriRegex(methodInfo);
        }

        private void InitializeParameterNames(MethodInfo methodInfo)
        {
            _parameterNames = methodInfo.GetParameters().Select(p => p.Name).ToArray();
        }

        private void InitializeInstantiator(MethodInfo methodInfo)
        {
            Instantiator = InstanceCreatorCache.GetCreator(methodInfo.DeclaringType);
        }

        private void InitializeMatchUriRegex(MethodInfo methodInfo)
        {
            var uriFormatter = methodInfo.GetCustomAttribute<UriFormatAttribute>();
            string regexToMatchUri = string.Format("^{0}$", FIND_PARAMETERKEYS_REGEX.Replace(uriFormatter.UriFormat, MATCHURI_REPLACE_STRING));
            _matchUriRegex = new Regex(regexToMatchUri);

        }

        private void InitializeFindParameterRegex(MethodInfo methodInfo)
        {
            var uriFormatter = methodInfo.GetCustomAttribute<UriFormatAttribute>();

            string regexToFindParamValues = string.Format("^{0}$", FIND_PARAMETERKEYS_REGEX.Replace(uriFormatter.UriFormat, MATCHPARAMETER_REPLACE_STRING));

            _findParameterValuesRegex = new Regex(regexToFindParamValues);
        }

        private void InitializeVerb(MethodInfo mi)
        {
            var restVerbAttr = mi.ReturnType.GetTypeInfo().GetCustomAttribute<RestVerbAttribute>();
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

            foreach (string parameterName in _parameterNames)
            {
                yield return m.Groups[parameterName].Value;
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
