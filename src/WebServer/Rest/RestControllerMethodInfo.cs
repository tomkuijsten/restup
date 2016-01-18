using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.Restup.WebServer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Devkoes.Restup.WebServer.Rest
{
    internal class RestControllerMethodInfo
    {
        private static readonly Regex FIND_PARAMETERKEYS_REGEX = new Regex("{(.*?)}", RegexOptions.Compiled);
        private const string MATCHPARAMETER_REPLACE_STRING = "(?<$1>.+?)";
        private const string MATCHURI_REPLACE_STRING = ".+?";

        private IEnumerable<Type> _validParameterTypes;
        private Regex _findParameterValuesRegex;
        private Regex _matchUriRegex;
        private IDictionary<string, Type> _parametersForUri;
        private string _urlPrefix;

        internal MethodInfo MethodInfo { get; private set; }
        internal HttpMethod Verb { get; private set; }
        internal bool HasBodyParameter { get; private set; }
        internal Type BodyParameterType { get; private set; }
        internal bool IsAsync { get; }
        internal Func<object[]> ControllerConstructorArgs { get; }

        internal RestControllerMethodInfo(
            MethodInfo methodInfo,
            string urlPrefix,
            Func<object[]> constructorArgs,
            bool isAsync)
        {
            constructorArgs.GuardNull(nameof(constructorArgs));

            _urlPrefix = urlPrefix;
            IsAsync = isAsync;
            ControllerConstructorArgs = constructorArgs;
            MethodInfo = methodInfo;

            InitializeValidParameterTypes();
            InitializeParameters();
            InitializeVerb();
            InitializeFindParameterRegex();
            InitializeMatchUriRegex();
            InitializeBodyParameter();
        }

        internal RestControllerMethodInfo(MethodInfo methodInfo, string urlPrefix, Func<object[]> constructorArgs) : this(methodInfo, urlPrefix, constructorArgs, false) { }

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
            string uriFormatWithPrefix = CreateUriFormat(uriFormatter);
            string regexToMatchUri = string.Format("^{0}$", FIND_PARAMETERKEYS_REGEX.Replace(uriFormatWithPrefix, MATCHURI_REPLACE_STRING));
            _matchUriRegex = new Regex(regexToMatchUri, RegexOptions.Compiled);

        }

        private void InitializeFindParameterRegex()
        {
            var uriFormatter = MethodInfo.GetCustomAttribute<UriFormatAttribute>();
            string uriFormatWithPrefix = CreateUriFormat(uriFormatter);
            string regexToFindParamValues = string.Format("^{0}$", FIND_PARAMETERKEYS_REGEX.Replace(uriFormatWithPrefix, MATCHPARAMETER_REPLACE_STRING));

            _findParameterValuesRegex = new Regex(regexToFindParamValues, RegexOptions.Compiled);
        }

        private string CreateUriFormat(UriFormatAttribute uriFormatter)
        {
            string uriFormat = uriFormatter.UriFormat.RemovePreAndPostSlash();

            if (string.IsNullOrWhiteSpace(_urlPrefix))
            {
                return "/" + uriFormat;
            }

            return $"{_urlPrefix}/{uriFormat}";
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

        internal bool Match(Uri uri)
        {
            return UriMatches(uri);
        }

        private bool UriMatches(Uri uri)
        {
            string relativeUri = uri.ToRelativeString();

            return _matchUriRegex.IsMatch(relativeUri);
        }

        internal IEnumerable<object> GetParametersFromUri(Uri uri)
        {
            Match m = _findParameterValuesRegex.Match(uri.ToRelativeString());
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
