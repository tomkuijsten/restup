using System;
using System.Collections.Concurrent;
using System.Text;

namespace Restup.Webserver
{
    internal class EncodingCache
    {
        internal static EncodingCache Default;

        static EncodingCache()
        {
            Default = new EncodingCache();
        }

        private readonly ConcurrentDictionary<string, Encoding> _cache;

        internal EncodingCache()
        {
            _cache = new ConcurrentDictionary<string, Encoding>(StringComparer.OrdinalIgnoreCase);
        }

        internal Encoding GetEncoding(string charset)
        {
            if (string.IsNullOrEmpty(charset))
            {
                return null;
            }

            return _cache.GetOrAdd(charset, ResolveEncoding);
        }

        private static Encoding ResolveEncoding(string charset)
        {
            try
            {
                return Encoding.GetEncoding(charset);
            }
            catch
            {
                return null;
            }
        }
    }
}
