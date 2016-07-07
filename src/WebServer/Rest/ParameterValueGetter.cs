using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Restup.Webserver.Rest
{
    internal abstract class ParameterValueGetter
    {
        private const char URIPARAMETER_ARRAY_SEPERATOR = ';';
        private readonly Type _parameterType;

        internal string MethodName { get; }

        protected ParameterValueGetter(string methodName, Type parameterType)
        {
            MethodName = methodName;
            _parameterType = parameterType;
        }

        internal object GetParameterValue(ParsedUri parsedUri)
        {
            var value = GetValueFromUri(parsedUri);
            var convertedValue = ConvertValueToParameterType(_parameterType, value);
            return convertedValue;
        }

        protected abstract string GetValueFromUri(ParsedUri parsedUri);

        private static object ConvertValueToParameterType(Type parameterType, string parameterValue)
        {
            if (parameterType == typeof(string))
            {
                // String is also an IEnumerable, but should not be treated as one
                return Convert.ChangeType(parameterValue, parameterType);
            }

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
}