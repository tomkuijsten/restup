using Restup.HttpMessage;
using System.Text;

namespace Restup.Webserver.Models.Schemas
{
    /// <summary>
    /// Wraps a <see cref="HttpServerRequest"/> with the defaults defined in the <see cref="Configuration"/>
    /// settings.
    /// </summary>
    internal struct RestServerRequest
    {
        internal IHttpServerRequest HttpServerRequest { get; private set; }

        internal string AcceptCharset { get; private set; }

        internal MediaType AcceptMediaType { get; private set; }

        internal Encoding AcceptEncoding { get; private set; }

        internal MediaType ContentMediaType { get; private set; }

        internal string ContentCharset { get; private set; }

        internal Encoding ContentEncoding { get; private set; }

        internal RestServerRequest(
            IHttpServerRequest httpServerRequest,
            string acceptCharset,
            MediaType acceptMediaType,
            Encoding acceptEncoding,
            MediaType contentMediaType,
            string contentCharset,
            Encoding contentEncoding)
        {
            this.HttpServerRequest = httpServerRequest;
            this.AcceptCharset = acceptCharset;
            this.AcceptMediaType = acceptMediaType;
            this.AcceptEncoding = acceptEncoding;
            this.ContentMediaType = contentMediaType;
            this.ContentCharset = contentCharset;
            this.ContentEncoding = contentEncoding;
        }
    }
}
