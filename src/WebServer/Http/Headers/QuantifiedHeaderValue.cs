using System.Collections.Generic;

namespace Devkoes.Restup.WebServer.Http.Headers
{
    internal class QuantifiedHeaderValue
    {
        internal string HeaderValue { get; }

        internal IDictionary<string, string> Quantifiers { get; }

        public QuantifiedHeaderValue(string headerValue, IDictionary<string, string> quantifiers)
        {
            HeaderValue = headerValue;
            Quantifiers = quantifiers;
        }

        internal string FindQuantifierValue(string quantifierKey)
        {
            if (Quantifiers.ContainsKey(quantifierKey))
            {
                return Quantifiers[quantifierKey];
            }

            return null;
        }
    }
}
