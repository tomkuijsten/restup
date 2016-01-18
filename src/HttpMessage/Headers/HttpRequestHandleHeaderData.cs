using Devkoes.HttpMessage.Models.Contracts;

namespace Devkoes.HttpMessage.Headers
{
    /// <summary>
    /// Set specific properties on the <see cref="HttpServerRequest"/> object based
    /// on the httpheader.
    /// </summary>
    /// <remarks>
    /// All methods in this class are thread safe
    /// </remarks>
    public class HttpRequestHandleHeaderData : IHttpHeaderVisitor<HttpServerRequest>
    {
        internal static HttpRequestHandleHeaderData Default { get; }

        static HttpRequestHandleHeaderData()
        {
            Default = new HttpRequestHandleHeaderData();
        }

        private HttpRequestHandleHeaderData()
        {

        }

        public void Visit(AcceptHeader uh, HttpServerRequest arg)
        {
            arg.ResponseContentType = uh.AcceptType;
        }

        public void Visit(AcceptCharsetHeader uh, HttpServerRequest arg)
        {
            arg.ResponseContentEncoding = uh.ResponseContentEncoding;
        }

        public void Visit(ContentTypeHeader uh, HttpServerRequest arg)
        {
            arg.RequestContentEncoding = uh.ContentEncoding;
            arg.RequestContentType = uh.ContentType;
        }

        public void Visit(ContentLengthHeader uh, HttpServerRequest arg)
        {
            arg.ContentLength = uh.Length;
        }

        public void Visit(UntypedHeader uh, HttpServerRequest arg)
        {
            // no specific info to set for untyped header
        }
    }
}
