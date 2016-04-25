using Restup.HttpMessage.Models.Contracts;

namespace Restup.HttpMessage.Headers.Request
{
    /// <summary>
    /// Set specific properties on the <see cref="MutableHttpServerRequest"/> object based
    /// on the httpheader.
    /// </summary>
    /// <remarks>
    /// All methods in this class are thread safe
    /// </remarks>
    public class HttpRequestHandleHeaderData : IHttpRequestHeaderVisitor<MutableHttpServerRequest>
    {
        internal static HttpRequestHandleHeaderData Default { get; }

        static HttpRequestHandleHeaderData()
        {
            Default = new HttpRequestHandleHeaderData();
        }

        private HttpRequestHandleHeaderData()
        {

        }

        public void Visit(AcceptHeader uh, MutableHttpServerRequest arg)
        {
            arg.AcceptMediaTypes = uh.AcceptTypes;
        }

        public void Visit(AcceptCharsetHeader uh, MutableHttpServerRequest arg)
        {
            arg.AcceptCharsets = uh.ResponseContentEncoding;
        }

        public void Visit(AcceptEncodingHeader uh, MutableHttpServerRequest arg)
        {
            arg.AcceptEncodings = uh.AcceptEncodings;
        }

        public void Visit(ContentTypeHeader uh, MutableHttpServerRequest arg)
        {
            arg.ContentTypeCharset = uh.ContentCharset;
            arg.ContentType = uh.ContentType;
        }

        public void Visit(ContentLengthHeader uh, MutableHttpServerRequest arg)
        {
            arg.ContentLength = uh.Length;
        }

        public void Visit(UntypedRequestHeader uh, MutableHttpServerRequest arg)
        {
            // no specific info to set for untyped header
        }
    }
}
