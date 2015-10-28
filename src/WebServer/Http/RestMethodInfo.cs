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
        private const string MATCHPARAMETER_REPLACE_STRING = "(?<$1>.+?)";
        private const string MATCHURI_REPLACE_STRING = ".+?";

        private IEnumerable<Type> _validParameterTypes;
        private Regex _findParameterValuesRegex;
        private Regex _matchUriRegex;
        private IDictionary<string, Type> _parametersForUri;

        public MethodInfo MethodInfo { get; private set; }
        public RestVerb Verb { get; private set; }
        public bool HasBodyParameter { get; private set; }
        public Type BodyParameterType { get; private set; }
        public bool IsAsync { get; }

        public RestMethodInfo(MethodInfo methodInfo, bool isAsync)
        {
            IsAsync = isAsync;

            MethodInfo = methodInfo;

            InitializeValidParameterTypes();
            InitializeParameters();
            InitializeVerb();
            InitializeFindParameterRegex();
            InitializeMatchUriRegex();
            InitializeBodyParameter();
        }

        public RestMethodInfo(MethodInfo methodInfo) : this(methodInfo, false)
        {

        }

        private void InitializeValidParameterTypes()
        {
            _validParameterTypes = new[] {
                typeof(string),
                typeof(decimal),
                typeof(double),
                typeof(float),
                typeof(short),
                typeof(int),
                typeof(long),
                typeof(byte),
                typeof(bool),
                typeof(DateTime),
                typeof(char),
                typeof(sbyte),
                typeof(ushort),
                typeof(uint),
                typeof(ulong),
            };
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

        private void InitializeParameters()
        {
            var fromUriParams = from p in MethodInfo.GetParameters()
                                where p.GetCustomAttribute<FromBodyAttribute>() == null
                                select p;

            if (!ParametersHaveValidType(fromUriParams.Select(p => p.ParameterType)))
            {
                throw new InvalidOperationException("Can't use method parameters with a custom type.");
            }

            _parametersForUri = fromUriParams.ToDictionary(p => p.Name, p => p.ParameterType);
        }

        private bool ParametersHaveValidType(IEnumerable<Type> parameters)
        {
            return !parameters.Except(_validParameterTypes).Any();
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
            TypeInfo returnType = null;

            if (!IsAsync)
                returnType = MethodInfo.ReturnType.GetTypeInfo();
            else
                returnType = MethodInfo.ReturnType.GetGenericArguments()[0].GetTypeInfo();

            Verb = returnType.GetCustomAttribute<RestVerbAttribute>().Verb;
        }

        public bool Match(string uri)
        {
            return UriMatches(uri);
        }

        private bool UriMatches(string uri)
        {
            return _matchUriRegex.IsMatch(uri);
        }

        public IEnumerable<object> GetParametersFromUri(string uri)
        {
            Match m = _findParameterValuesRegex.Match(uri);
            if (!m.Success)
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
