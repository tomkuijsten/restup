using Devkoes.HttpMessage;
using Devkoes.HttpMessage.Models.Schemas;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Devkoes.HttpMessage.Plumbing
{
    internal static class Extensions
    {
        private static Regex _trimStart = new Regex(@"^\s+", RegexOptions.Compiled);
        private static Regex _trimEnd = new Regex(@"\s+$", RegexOptions.Compiled);

        internal static string TrimWhitespaces(this string value)
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

        /// <summary>
        /// Read the next sequence of bytes untill a space or a CRLF is reached
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        internal static ExtractedWord ReadNextWord(this byte[] stream)
        {
            for (int i = 0; i < stream.Length; i++)
            {
                byte currentByte = stream[i];
                if (currentByte == Constants.SpaceByte)
                {
                    return new ExtractedWord()
                    {
                        Word = Constants.DefaultHttpMessageCharset.GetString(stream.Take(i).ToArray()),
                        RemainingBytes = stream.Skip(i + 1).ToArray(),
                        WordFound = true
                    };
                }
                else if (currentByte == Constants.CRByte)
                {
                    byte next = stream.Length > (i + 1) ? stream[i + 1] : (byte)0;
                    if (next == Constants.LFByte)
                    {
                        return new ExtractedWord()
                        {
                            Word = Constants.DefaultHttpMessageCharset.GetString(stream.Take(i).ToArray()),
                            RemainingBytes = stream.Skip(i + 2).ToArray(),
                            WordFound = true
                        };

                    }
                }
            }

            return new ExtractedWord() { RemainingBytes = stream };
        }

        internal static T[] ConcatArray<T>(this T[] array1, T[] array2)
        {
            int array1OriginalLength = array1.Length;
            Array.Resize<T>(ref array1, array1OriginalLength + array2.Length);
            Array.Copy(array2, 0, array1, array1OriginalLength, array2.Length);

            return array1;
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
