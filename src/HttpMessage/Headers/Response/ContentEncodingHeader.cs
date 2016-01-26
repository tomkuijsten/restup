using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.HttpMessage.Plumbing;

namespace Devkoes.HttpMessage.Headers.Response
{
    public class ContentTypeHeader : HttpHeaderBase
    {
        internal static string NAME = "Content-Type";
        private static string CHARSET_KEY = "charset";

        private MediaType _contentType;
        private string _charset;

        public ContentTypeHeader(MediaType contentType, string charset) :
            base(NAME, ContentTypeHeader.FormatContentType(contentType, charset))
        {
            Charset = charset;
            ContentType = contentType;
        }

        public ContentTypeHeader(MediaType contentType) : this(contentType, null) { }

        public MediaType ContentType
        {
            get { return _contentType; }
            set
            {
                _contentType = value;
                Value = FormatContentType(_contentType, _charset);
            }
        }

        public string Charset
        {
            get { return _charset; }
            set
            {
                _charset = value;
                Value = FormatContentType(_contentType, _charset);
            }
        }

        private static string FormatContentType(MediaType contentType, string charset)
        {
            string charsetPart = string.IsNullOrEmpty(charset) ? string.Empty : $";{CHARSET_KEY}={charset}";
            return string.Concat(HttpCodesTranslator.Default.GetMediaType(contentType), charsetPart);
        }
    }
}
