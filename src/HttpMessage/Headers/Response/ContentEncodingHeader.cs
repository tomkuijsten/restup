using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.HttpMessage.Plumbing;

namespace Devkoes.HttpMessage.Headers.Response
{
    public class ContentTypeHeader : HttpHeaderBase
    {
        private const string NAME = "Content-Type";
        private const string CHARSET_KEY = "charset";

        public MediaType ContentType { get; }
        public string Charset { get; }

        public ContentTypeHeader(MediaType contentType, string charset) :
            base(NAME, FormatContentType(contentType, charset))
        {
            Charset = charset;
            ContentType = contentType;
        }

        public ContentTypeHeader(MediaType contentType) : this(contentType, null) { }

        private static string FormatContentType(MediaType contentType, string charset)
        {
            var charsetPart = string.IsNullOrEmpty(charset) ? string.Empty : $";{CHARSET_KEY}={charset}";
            return string.Concat(HttpCodesTranslator.Default.GetMediaType(contentType), charsetPart);
        }
    }
}
