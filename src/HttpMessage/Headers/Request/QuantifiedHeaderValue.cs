using System;
using System.Collections.Generic;

namespace Restup.HttpMessage.Headers.Request
{
    public class QuantifiedHeaderValue
    {
        private static string QUALITY_KEY = "q";

        internal string HeaderValue { get; }

        internal IDictionary<string, string> Quantifiers { get; }

        internal decimal Quality { get; private set; }

        public QuantifiedHeaderValue(string headerValue, IDictionary<string, string> quantifiers)
        {
            HeaderValue = headerValue;
            Quantifiers = quantifiers;

            ExtractQuality();
        }

        private void ExtractQuality()
        {
            if (Quantifiers.ContainsKey(QUALITY_KEY))
            {
                decimal qualityAsDec;
                if (decimal.TryParse(Quantifiers[QUALITY_KEY], out qualityAsDec))
                {
                    Quality = Math.Min(Math.Max(qualityAsDec, 0), 1);
                }
                else
                {
                    Quality = 0;
                }
            }
            else
            {
                Quality = 1;
            }
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
