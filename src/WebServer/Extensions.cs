using System;
using System.IO;
using Windows.ApplicationModel;

namespace Restup.Webserver
{
    internal static class Extensions
    {
        internal static string RemovePreAndPostSlash(this string uri)
        {
            if (uri == null)
                return uri;

            return uri.TrimStart('/').TrimEnd('/');
        }

        internal static string EscapeRegexChars(this string uri)
        {
            if (uri == null)
                return uri;

            return uri.Replace("?", "\\?");
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

        internal static Uri RemovePrefix(this Uri uri, string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                return uri;

            var uriToString = uri.ToString();
            if (uriToString.StartsWith(prefix))
                uriToString = uriToString.Remove(0, prefix.Length);

            return new Uri(uriToString, UriKind.Relative);
        }

        internal static string GetAbsoluteBasePathUri(this string relativeOrAbsoluteBasePath)
        {
            relativeOrAbsoluteBasePath = relativeOrAbsoluteBasePath ?? string.Empty;

            relativeOrAbsoluteBasePath = relativeOrAbsoluteBasePath.TrimStart('\\');

            if (Path.IsPathRooted(relativeOrAbsoluteBasePath))
                return relativeOrAbsoluteBasePath;

            return Path.Combine(Package.Current.InstalledLocation.Path, relativeOrAbsoluteBasePath);
        }
    }
}
