namespace Restup.HttpMessage.Headers.Response
{
    public class ContentEncodingHeader : HttpHeaderBase
    {
        internal static string NAME = "Content-Encoding";

        public string Encoding { get; }

        public ContentEncodingHeader(string encoding) : base(NAME, encoding)
        {
            Encoding = encoding;
        }
    }
}