using Devkoes.Restup.WebServer.Http;
using System.Text;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    interface IHttpHeaderVisitor<T>
    {
        void Visit(UntypedHeader uh, T arg);
        void Visit(ContentLengthHeader uh, T arg);
        void Visit(AcceptHeader uh, T arg);
        void Visit(ContentTypeHeader uh, T arg);
        void Visit(AcceptCharsetHeader uh, T arg);
    }

    internal interface IHttpHeader
    {
        string Name { get; set; }
        string RawContent { get; set; }

        void Visit<T>(IHttpHeaderVisitor<T> v, T arg);
    }

    internal abstract class BaseHeader : IHttpHeader
    {
        public string Name { get; set; }
        public string RawContent { get; set; }

        public BaseHeader(string name, string value)
        {
            Name = name;
            RawContent = value;
        }

        public abstract void Visit<T>(IHttpHeaderVisitor<T> v, T arg);
    }

    internal class UntypedHeader : BaseHeader
    {
        public UntypedHeader(string name, string value) : base(name, value)
        {

        }

        public override void Visit<T>(IHttpHeaderVisitor<T> v, T arg)
        {
            v.Visit(this, arg);
        }
    }

    internal class ContentLengthHeader : BaseHeader
    {
        internal static string NAME = "Content-Length";
        public int Length { get; set; }

        public ContentLengthHeader(string value) : base(NAME, value)
        {
            Length = int.Parse(value);
        }

        public override void Visit<T>(IHttpHeaderVisitor<T> v, T arg)
        {
            v.Visit(this, arg);
        }
    }

    internal class AcceptHeader : BaseHeader
    {
        internal static string NAME = "Accept";

        public MediaType AcceptType { get; set; }

        public AcceptHeader(string value) : base(NAME, value)
        {
            AcceptType = HttpCodesTranslator.GetMediaType(value);
        }

        public override void Visit<T>(IHttpHeaderVisitor<T> v, T arg)
        {
            v.Visit(this, arg);
        }
    }

    internal class ContentTypeHeader : BaseHeader
    {
        internal static string NAME = "Content-Type";

        public MediaType ContentType { get; internal set; }
        public Encoding ContentEncoding { get; internal set; }

        public ContentTypeHeader(string value) : base(NAME, value)
        {
            ContentType = HttpCodesTranslator.GetMediaType(value);
        }

        public override void Visit<T>(IHttpHeaderVisitor<T> v, T arg)
        {
            v.Visit(this, arg);
        }
    }

    internal class AcceptCharsetHeader : BaseHeader
    {
        internal static string NAME = "Accept-Charset";

        public Encoding ResponseContentEncoding { get; set; }

        public AcceptCharsetHeader(string value) : base(NAME, value)
        {
            ResponseContentEncoding = Encoding.GetEncoding(value);
        }

        public override void Visit<T>(IHttpHeaderVisitor<T> v, T arg)
        {
            v.Visit(this, arg);
        }
    }
}
