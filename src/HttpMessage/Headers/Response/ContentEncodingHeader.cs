using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.HttpMessage.Plumbing;
using System.Text;

namespace Devkoes.HttpMessage.Headers.Response
{
    public class ContentTypeHeader : HttpHeaderBase
    {
        internal static string NAME = "Content-Type";
        private static string CHARSET_KEY = "charset";

        private MediaType _contentType;
        private string _charset;

        public Encoding Encoding { get; private set; }

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
                UpdateEncoder();
            }
        }

        public string Charset
        {
            get { return _charset; }
            set
            {
                _charset = value;
                Value = FormatContentType(_contentType, _charset);
                UpdateEncoder();
            }
        }

        private void UpdateEncoder()
        {
            if (!string.IsNullOrWhiteSpace(_charset))
            {
                Encoding = Encoding.GetEncoding(Charset);
            }
            else if (ContentType == MediaType.JSON)
            {
                Encoding = Constants.DefaultJSONEncoding;
            }
            else
            {
                Encoding = Constants.DefaultHttpEncoding;
            }
        }

        private static string FormatContentType(MediaType contentType, string charset)
        {
            string charsetPart = string.IsNullOrEmpty(charset) ? string.Empty : $";{CHARSET_KEY}={charset}";
            return string.Concat(HttpCodesTranslator.Default.GetMediaType(contentType), charsetPart);
        }
    }
}
