namespace Restup.HttpMessage.Headers.Response
{
    public class ContentTypeHeader : HttpHeaderBase
    {
        private const string NAME = "Content-Type";
        private const string CHARSET_KEY = "charset";

        public string ContentType { get; }
        public string Charset { get; }

        public ContentTypeHeader(string contentType, string charset) :
            base(NAME, FormatContentType(contentType, charset))
        {
            Charset = charset;
            ContentType = contentType;
        }

        public ContentTypeHeader(string contentType) : this(contentType, null) { }

        private static string FormatContentType(string contentType, string charset)
        {
            var charsetPart = string.IsNullOrEmpty(charset) ? string.Empty : $";{CHARSET_KEY}={charset}";
            return string.Concat(contentType, charsetPart);
        }
    }
}
