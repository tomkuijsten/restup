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
        private IReadOnlyDictionary<string, int> _pathParameterNameToIndex;
        private IReadOnlyDictionary<string, string> _queryParameterNamesByMethodName;
        private readonly UriParser uriParser;
        private ParsedUri matchUri;

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
            ParsedUri originalParsedUri;
            if (!uriParser.TryParse(uriFormatter.UriFormat, out originalParsedUri))
                throw new Exception($"Could not parse uri: {uriFormatter.UriFormat}");

            matchUri = originalParsedUri;
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

            _pathParameterNameToIndex = matchUri.PathParts
                .Select((x, i) => new { Part = x, Index = i })
                .Where(x => x.Part.PartType == PathPart.PathPartType.Argument)
                .Where(x => _parametersForUri.Any(y => y.Key.Equals(x.Part.Value, StringComparison.OrdinalIgnoreCase)))
                .ToDictionary(x => x.Part.Value, x => x.Index, StringComparer.OrdinalIgnoreCase);

            _queryParameterNamesByMethodName = matchUri.Parameters
                .Where(x => _parametersForUri.Any(y => y.Key.Equals(x.Value, StringComparison.OrdinalIgnoreCase)))
                .ToDictionary(x => x.Value, x => x.Name, StringComparer.OrdinalIgnoreCase);
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
            if (matchUri.PathParts.Count != uri.PathParts.Count)
                return false;

            for (var i = 0; i < matchUri.PathParts.Count; i++)
            {
                var fromPart = matchUri.PathParts[i];
                var toPart = uri.PathParts[i];
                if (fromPart.PartType == PathPart.PathPartType.Argument)
                    continue;

                if (!fromPart.Value.Equals(toPart.Value, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            if (uri.Parameters.Count < matchUri.Parameters.Count)
                return false;

            return matchUri.Parameters.All(x => uri.Parameters.Any(y => y.Name.Equals(x.Name, StringComparison.OrdinalIgnoreCase)));
        }

        internal IEnumerable<object> GetParametersFromUri(Uri uri) // TODO:  pass in ParsedUri
        {
            var paramValues = new Dictionary<string, object>();

            ParsedUri parsedUri;
            if (!uriParser.TryParse(uri.ToRelativeString(), out parsedUri))
            {
                return Enumerable.Empty<object>();
            }

            foreach (var parameter in _pathParameterNameToIndex)
            {
                paramValues.Add(parameter.Key, HandleParameter(_parametersForUri[parameter.Key], parsedUri.PathParts[parameter.Value].Value));
            }

            foreach (var parameter in _queryParameterNamesByMethodName)
            {
                paramValues.Add(parameter.Key, HandleParameter(_parametersForUri[parameter.Key], parsedUri.Parameters.FirstOrDefault(x => x.Name.Equals(parameter.Value, StringComparison.OrdinalIgnoreCase)).Value));
            }

            return _parametersForUri.Select(kv => paramValues[kv.Key]).ToArray();
        }

        private static object HandleParameter(Type parameterType, string parameterValue)
        {
            if (parameterType == typeof(string))
            {
                // String is also an IEnumerable, but should not be treated as one
                return Convert.ChangeType(parameterValue, parameterType);
            }
            else
            {
                if (typeof(IEnumerable).IsAssignableFrom(parameterType))
                {
                    // Because we are in control of the allowed types (_validParameterTypes) we are sure that
                    // there will always be a generic argument. Get index 0  is safe.
                    var genericType = parameterType.GenericTypeArguments[0];
                    var genericListType = typeof(List<>).MakeGenericType(genericType);
                    var genericList = (IList)Activator.CreateInstance(genericListType);

                    var uriValue = parameterValue;
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

                return Convert.ChangeType(parameterValue, parameterType);
            }
        }

        public override string ToString()
        {
            return $"Hosting {Verb} method on {matchUri}";
        }
    }
}
