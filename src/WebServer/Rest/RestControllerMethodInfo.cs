using Restup.HttpMessage.Models.Schemas;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Restup.Webserver.Rest
{
    internal class RestControllerMethodInfo
    {
        internal enum TypeWrapper
        {
            None,
            Task,
            AsyncOperation
        }

        private const char URIPARAMETER_ARRAY_SEPERATOR = ';';

        private IEnumerable<Type> _validParameterTypes;

        private IDictionary<string, Type> _parametersForUri;
        private IEnumerable<string> _pathParameterNames;
        private IEnumerable<string> _queryParameterNames;
        private readonly UriParser uriParser;
        private MatchUri matchUri;

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
            uriParser = new UriParser();
            SetUriInfo(methodInfo);

            ReturnTypeWrapper = typeWrapper;
            ControllerConstructorArgs = constructorArgs;
            MethodInfo = methodInfo;

            InitializeValidParameterTypes();
            InitializeParameters();
            InitializeVerb();

            InitializeContentParameter();
        }

        private void SetUriInfo(MethodInfo methodInfo)
        {
            var uriFormatter = methodInfo.GetCustomAttribute<UriFormatAttribute>();
            var originalParsedUri = uriParser.Parse(uriFormatter.UriFormat);            

            matchUri = new MatchUri(originalParsedUri);
        }

        private void InitializeValidParameterTypes()
        {
            _validParameterTypes = new[] {
                typeof(IEnumerable<byte>),
                typeof(IEnumerable<sbyte>),
                typeof(IEnumerable<short>),
                typeof(IEnumerable<ushort>),
                typeof(IEnumerable<int>),
                typeof(IEnumerable<uint>),
                typeof(IEnumerable<long>),
                typeof(IEnumerable<ulong>),
                typeof(IEnumerable<decimal>),
                typeof(IEnumerable<double>),
                typeof(IEnumerable<float>),
                typeof(IEnumerable<string>),
                typeof(IEnumerable<bool>),
                typeof(IEnumerable<char>),
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

            var pathParams = new List<string>();
            var queryParams = new List<string>();
            foreach (var item in _parametersForUri)
            {
                if (matchUri.Parameters.Any(x => x.Value.Equals(item.Key, StringComparison.OrdinalIgnoreCase)))
                {
                    queryParams.Add(item.Key);
                }
                else
                {
                    pathParams.Add(item.Key);
                }
            }

            _pathParameterNames = pathParams;
            _queryParameterNames = queryParams;
        }

        private bool ParametersHaveValidType(IEnumerable<Type> parameters)
        {
            return !parameters.Except(_validParameterTypes).Any();
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

        internal bool Match(ParsedUri uri)
        {
            return UriMatches(uri);
        }

        private bool UriMatches(ParsedUri uri)
        {
            return string.Equals(matchUri.Path, uri.Path, StringComparison.OrdinalIgnoreCase);
        }

        internal IEnumerable<object> GetParametersFromUri(Uri uri)
        {
            var paramValues = new Dictionary<string, object>();

            string baseUri = uri.ToRelativeString();
            int queryIndex = baseUri.IndexOf('?');
            string localPath = null;
            if (queryIndex > -1)
            {
                localPath = baseUri.Substring(0, queryIndex);
            }
            else
            {
                localPath = baseUri.ToString();
            }

            Match pathParametersMatch = _findPathParameterValuesRegex.Match(localPath);
            if (!pathParametersMatch.Success)
            {
                return Enumerable.Empty<object>();
            }

            foreach (var parameter in _pathParameterNames)
            {
                paramValues.Add(parameter, HandleParameter(parameter, _parametersForUri[parameter], pathParametersMatch));
            }

            if (_hasQueryParameter)
            {
                string query = baseUri.ToString().Substring(queryIndex);
                Match queryParametersMatch = _findQueryParameterValuesRegex.Match(query);
                foreach (var parameter in _queryParameterNames)
                {
                    paramValues.Add(parameter, HandleParameter(parameter, _parametersForUri[parameter], queryParametersMatch));
                }
            }

            return _parametersForUri.Select(kv => paramValues[kv.Key]).ToArray();
        }

        private object HandleParameter(string parameterName, Type parameterType, Match matchedRegex)
        {
            if (parameterType == typeof(string))
            {
                // String is also an IEnumerable, but should not be treated as one
                return Convert.ChangeType(matchedRegex.Groups[parameterName].Value, parameterType);
            }
            else if (typeof(IEnumerable).IsAssignableFrom(parameterType))
            {
                // Because we are in control of the allowed types (_validParameterTypes) we are sure that
                // there will always be a generic argument. Get index 0  is safe.
                var genericType = parameterType.GenericTypeArguments[0];
                var genericListType = typeof(List<>).MakeGenericType(genericType);
                var genericList = (IList)Activator.CreateInstance(genericListType);

                var uriValue = matchedRegex.Groups[parameterName].Value;
                foreach (var v in uriValue.Split(URIPARAMETER_ARRAY_SEPERATOR))
                {
                    if (genericType == typeof(string))
                    {
                        string d = (string)Convert.ChangeType(v, genericType);
                        genericList.Add(Uri.UnescapeDataString(d));
                    }
                    else
                    {
                        genericList.Add(Convert.ChangeType(v, genericType));
                    }
                }

                return genericList;
            }
            else
            {
                return Convert.ChangeType(matchedRegex.Groups[parameterName].Value, parameterType);
            }
        }

        public override string ToString()
        {
            return $"Hosting {Verb.ToString()} method on {matchUri}";
        }
    }

    internal class MatchUri
    {
        public string Path { get; }
        internal MatchUri(ParsedUri originalParsedUri)
        {

            Path = originalParsedUri.Path;

            PathVariables = GetVariablesFromPath(Path);
        }

        private string GetVariablesFromPath(string path)
        {
            
        }
    }

    internal class MatchUriParser
    {
        private Regex pathVariablesRegex;

        public MatchUriParser()
        {
            pathVariablesRegex = new Regex(@"/(?<vars>\{.*?\}(?=/))");

        }
        internal MatchUri Parse(ParsedUri parsedUri)
        {
            
        }
    }
}
