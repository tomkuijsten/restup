using Restup.HttpMessage.Models.Schemas;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Restup.HttpMessage.Plumbing
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
                        Word = Constants.DefaultHttpEncoding.GetString(stream.Take(i).ToArray()),
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
                            Word = Constants.DefaultHttpEncoding.GetString(stream.Take(i).ToArray()),
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
    }
}
