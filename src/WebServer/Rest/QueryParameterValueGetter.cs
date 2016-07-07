using System;
using System.Linq;

namespace Restup.Webserver.Rest
{
    internal class QueryParameterValueGetter : ParameterValueGetter
    {
        private readonly string _queryParameterName;

        public QueryParameterValueGetter(string methodName, Type parameterType, UriParameter uriParameter) 
            : base(methodName, parameterType)
        {
            _queryParameterName = uriParameter.Name;
        }

        protected override string GetValueFromUri(ParsedUri parsedUri)
        {
            return parsedUri.Parameters.FirstOrDefault(x => x.Name.Equals(_queryParameterName, StringComparison.OrdinalIgnoreCase)).Value;
        }
    }
}