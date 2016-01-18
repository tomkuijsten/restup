using System;

namespace Devkoes.Restup.WebServer
{
    internal static class Extensions
    {
        internal static string RemovePreAndPostSlash(this string uri)
        {
            if (uri == null)
                return uri;

            return uri.TrimStart('/').TrimEnd('/');
        }

        /// <summary>
        /// The prefix will always be formatted as "/prefix"
        /// </summary>
        internal static string FormatRelativeUri(this string uri)
        {
            var cleanUrl = uri.RemovePreAndPostSlash();

            return string.IsNullOrWhiteSpace(cleanUrl) ? string.Empty : "/" + cleanUrl;
        }

        internal static string ToRelativeString(this Uri uri)
        {
            string relativeUri = null;
            if (uri.IsAbsoluteUri)
            {
                relativeUri = uri.PathAndQuery;
            }
            else
            {
                relativeUri = uri.ToString();
            }

            return relativeUri.FormatRelativeUri();
        }

        internal static void GuardNull(this object argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}
