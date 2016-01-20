using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.HttpMessage.Plumbing;
using System.Text;

namespace Devkoes.HttpMessage.Headers.Response
{
    public class ContentTypeHeader : HttpHeaderBase
    {
        internal static string NAME = "Content-Type";

        private MediaType _contentType;
        private string _charset;

        public Encoder Encoder { get; private set; }

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

                if (!string.IsNullOrWhiteSpace(value))
                {
                    Encoder = Encoding.GetEncoding(Charset).GetEncoder();
                }

                Value = FormatContentType(_contentType, _charset);
            }
        }

        private static string FormatContentType(MediaType contentType, string charset)
        {
            string charsetPart = string.IsNullOrEmpty(charset) ? string.Empty : $";charset={charset}";
            return string.Concat(HttpCodesTranslator.Default.GetMediaType(contentType), charsetPart);
        }
    }
}
