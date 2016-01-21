using Devkoes.HttpMessage.Models.Contracts;

namespace Devkoes.HttpMessage.Headers.Request
{
    /// <summary>
    /// Set specific properties on the <see cref="HttpServerRequest"/> object based
    /// on the httpheader.
    /// </summary>
    /// <remarks>
    /// All methods in this class are thread safe
    /// </remarks>
    public class HttpRequestHandleHeaderData : IHttpRequestHeaderVisitor<HttpServerRequest>
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
            arg.AcceptMediaTypes = uh.AcceptTypes;
        }

        public void Visit(AcceptCharsetHeader uh, HttpServerRequest arg)
        {
            arg.AcceptCharsets = uh.ResponseContentEncoding;
        }

        public void Visit(ContentTypeHeader uh, HttpServerRequest arg)
        {
            arg.RequestContentEncoding = uh.ContentEncoding;
            arg.ContentType = uh.ContentType;
        }

        public void Visit(ContentLengthHeader uh, HttpServerRequest arg)
        {
            arg.ContentLength = uh.Length;
        }

        public void Visit(UntypedRequestHeader uh, HttpServerRequest arg)
        {
            // no specific info to set for untyped header
        }
    }
}
