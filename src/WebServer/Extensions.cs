using System.Text.RegularExpressions;

namespace Devkoes.Restup.WebServer
{
    internal static class Extensions
    {
        private static Regex _trimStart = new Regex(@"^\s+", RegexOptions.Compiled);
        private static Regex _trimEnd = new Regex(@"\s+$", RegexOptions.Compiled);

        public static string TrimWhitespaces(this string value)
        {
            if (value == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            value = _trimStart.Replace(value, string.Empty);
            value = _trimStart.Replace(value, string.Empty);

            return value;
        }
    }
}
