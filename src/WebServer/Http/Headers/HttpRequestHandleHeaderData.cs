using Devkoes.Restup.WebServer.Models.Contracts;

namespace Devkoes.Restup.WebServer.Http.Headers
{
    /// <summary>
    /// Set specific properties on the <see cref="HttpRequest"/> object based
    /// on the httpheader.
    /// </summary>
    /// <remarks>
    /// All methods in this class are thread safe
    /// </remarks>
    internal class HttpRequestHandleHeaderData : IHttpHeaderVisitor<HttpRequest>
    {
        internal static HttpRequestHandleHeaderData Default { get; }

        static HttpRequestHandleHeaderData()
        {
            Default = new HttpRequestHandleHeaderData();
        }

        private HttpRequestHandleHeaderData()
        {

        }

        public void Visit(AcceptHeader uh, HttpRequest arg)
        {
            arg.ResponseContentType = uh.AcceptType;
        }

        public void Visit(AcceptCharsetHeader uh, HttpRequest arg)
        {
            arg.ResponseContentEncoding = uh.ResponseContentEncoding;
        }

        public void Visit(ContentTypeHeader uh, HttpRequest arg)
        {
            arg.RequestContentEncoding = uh.ContentEncoding;
            arg.RequestContentType = uh.ContentType;
        }

        public void Visit(ContentLengthHeader uh, HttpRequest arg)
        {
            arg.ContentLength = uh.Length;
        }

        public void Visit(UntypedHeader uh, HttpRequest arg)
        {
            // no specific info to set for untyped header
        }
    }
}
