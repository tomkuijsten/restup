using Restup.HttpMessage.Models.Schemas;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Restup.Webserver.Rest
{
    internal class RestControllerMethodInfo
    {
        private static readonly Regex FIND_PARAMETERKEYS_REGEX = new Regex("{(.*?)}", RegexOptions.Compiled);
        private const string MATCHPARAMETER_REPLACE_STRING = "(?<$1>.+?)";
        private const string MATCHURI_REPLACE_STRING = ".+?";

        private IEnumerable<Type> _validParameterTypes;
        private Regex _findParameterValuesRegex;
        private string _urlToMatch;
        private Regex _matchUriRegex;
        private IDictionary<string, Type> _parametersForUri;

        internal MethodInfo MethodInfo { get; private set; }
        internal HttpMethod Verb { get; private set; }
        internal bool HasContentParameter { get; private set; }
        internal Type ContentParameterType { get; private set; }
        internal TypeWrapper ReturnTypeWrapper { get; }
        internal Func<object[]> ControllerConstructorArgs { get; }

        internal RestControllerMethodInfo(
            MethodInfo methodInfo,
            Func<object[]> constructorArgs,
            TypeWrapper typeWrapper)
        {
            constructorArgs.GuardNull(nameof(constructorArgs));

            ReturnTypeWrapper = typeWrapper;
            ControllerConstructorArgs = constructorArgs;
            MethodInfo = methodInfo;

            InitializeValidParameterTypes();
            InitializeParameters();
            InitializeVerb();

            GetUrlToMatch(methodInfo);
            InitializeFindParameterRegex(_urlToMatch);

            InitializeMatchUriRegex();
            InitializeContentParameter();
        }

        private void GetUrlToMatch(MethodInfo methodInfo)
        {
            var uriFormatter = methodInfo.GetCustomAttribute<UriFormatAttribute>();
            _urlToMatch = CreateUriFormat(uriFormatter);
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

        private void InitializeContentParameter()
        {
            var fromContentParameter = MethodInfo.GetParameters().FirstOrDefault((p) => p.GetCustomAttribute<FromContentAttribute>() != null);
            if (fromContentParameter == null)
            {
                return;
            }

            HasContentParameter = true;
            ContentParameterType = fromContentParameter.ParameterType;
        }

        private void InitializeParameters()
        {
            var fromUriParams = from p in MethodInfo.GetParameters()
                                where p.GetCustomAttribute<FromContentAttribute>() == null
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

        private void InitializeFindParameterRegex(string uriFormatWithPrefix)
        {
            string regexToFindParamValues = string.Format("^{0}$", FIND_PARAMETERKEYS_REGEX.Replace(uriFormatWithPrefix, MATCHPARAMETER_REPLACE_STRING));

            _findParameterValuesRegex = new Regex(regexToFindParamValues, RegexOptions.Compiled);
        }

        private string CreateUriFormat(UriFormatAttribute uriFormatter)
        {
            string uriFormat = uriFormatter.UriFormat.RemovePreAndPostSlash().EscapeRegexChars(); ;
            return string.Format("/{0}", uriFormat);
        }

        private void InitializeVerb()
        {
            TypeInfo returnType = null;

            if (ReturnTypeWrapper == TypeWrapper.None)
                returnType = MethodInfo.ReturnType.GetTypeInfo();
            else
                returnType = MethodInfo.ReturnType.GetGenericArguments()[0].GetTypeInfo();

            Verb = GetVerb(returnType);
        }

        private HttpMethod GetVerb(TypeInfo returnType)
        {
            if (IsRestResponseOfType<IGetResponse>(returnType))
                return HttpMethod.GET;
            if (IsRestResponseOfType<IPostResponse>(returnType))
                return HttpMethod.POST;
            if (IsRestResponseOfType<IPutResponse>(returnType))
                return HttpMethod.PUT;
            if (IsRestResponseOfType<IDeleteResponse>(returnType))
                return HttpMethod.DELETE;

            throw new ArgumentException($"Verb for return type {returnType} not know. Please use only {typeof(IGetResponse)}, {typeof(IPostResponse)}, {typeof(IPutResponse)}, {typeof(IDeleteResponse)} as return types.");
        }

        private static bool IsRestResponseOfType<T>(TypeInfo returnType)
        {
            return returnType.ImplementedInterfaces.Contains(typeof(T)) || returnType.AsType() == typeof(T);
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

        public override string ToString()
        {
            return $"Hosting {Verb.ToString()} method on {_urlToMatch}";
        }

        internal enum TypeWrapper
        {
            None,
            Task,
            AsyncOperation
        }
    }
}
