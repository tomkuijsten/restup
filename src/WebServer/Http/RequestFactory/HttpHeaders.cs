using Devkoes.Restup.WebServer.Models.Schemas;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Devkoes.Restup.WebServer.Http.RequestFactory
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

    internal abstract class HttpHeaderBase : IHttpHeader
    {
        public string Name { get; set; }
        public string RawContent { get; set; }

        public HttpHeaderBase(string name, string value)
        {
            Name = name;
            RawContent = value;
        }

        public abstract void Visit<T>(IHttpHeaderVisitor<T> v, T arg);
    }

    internal abstract class HttpMultiQuantifiedHeaderBase : HttpHeaderBase
    {
        public IEnumerable<QuantifiedHeaderValue> QuantifiedHeaderValues { get; set; }

        public HttpMultiQuantifiedHeaderBase(
            string name,
            string value,
            IEnumerable<QuantifiedHeaderValue> quantifiedHeaderValues) : base(name, value)
        {
            QuantifiedHeaderValues = quantifiedHeaderValues;
        }
    }

    internal abstract class HttpSingleQuantifiedHeaderBase : HttpHeaderBase
    {
        public QuantifiedHeaderValue QuantifiedHeaderValue { get; set; }

        public HttpSingleQuantifiedHeaderBase(string name, string value, QuantifiedHeaderValue quantifiedHeaderValue)
            : base(name, value)
        {
            QuantifiedHeaderValue = quantifiedHeaderValue;

        }
    }

    internal class QuantifiedHeaderValue
    {
        internal string HeaderValue { get; }
        internal IDictionary<string, string> Quantifiers { get; }

        public QuantifiedHeaderValue(string headerValue, IDictionary<string, string> quantifiers)
        {
            HeaderValue = headerValue;
            Quantifiers = quantifiers;
        }

        internal string FindQuantifierValue(string quantifierKey)
        {
            if (Quantifiers.ContainsKey(quantifierKey))
            {
                return Quantifiers[quantifierKey];
            }

            return null;
        }
    }

    internal class UntypedHeader : HttpHeaderBase
    {
        public UntypedHeader(string name, string value) : base(name, value)
        {

        }

        public override void Visit<T>(IHttpHeaderVisitor<T> v, T arg)
        {
            v.Visit(this, arg);
        }
    }

    internal class ContentLengthHeader : HttpHeaderBase
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

    internal class AcceptHeader : HttpMultiQuantifiedHeaderBase
    {
        internal static string NAME = "Accept";

        public MediaType AcceptType { get; set; }

        public AcceptHeader(string value, IEnumerable<QuantifiedHeaderValue> quantifiedHeaderValues) : base(NAME, value, quantifiedHeaderValues)
        {
            AcceptType = HttpCodesTranslator.GetMediaType(quantifiedHeaderValues.First().HeaderValue);
        }

        public override void Visit<T>(IHttpHeaderVisitor<T> v, T arg)
        {
            v.Visit(this, arg);
        }
    }

    internal class ContentTypeHeader : HttpSingleQuantifiedHeaderBase
    {
        internal static string NAME = "Content-Type";

        public MediaType ContentType { get; internal set; }
        public Encoding ContentEncoding { get; internal set; }

        public ContentTypeHeader(string value, QuantifiedHeaderValue quantifiedHeaderValue) : base(NAME, value, quantifiedHeaderValue)
        {
            ContentType = HttpCodesTranslator.GetMediaType(QuantifiedHeaderValue.HeaderValue);
            string charset = QuantifiedHeaderValue.FindQuantifierValue("charset");
            ContentEncoding = charset != null ? Encoding.GetEncoding(charset) : HttpConstants.DefaultHttpMessageCharset;
        }

        public override void Visit<T>(IHttpHeaderVisitor<T> v, T arg)
        {
            v.Visit(this, arg);
        }
    }

    internal class AcceptCharsetHeader : HttpMultiQuantifiedHeaderBase
    {
        internal static string NAME = "Accept-Charset";

        public Encoding ResponseContentEncoding { get; set; }

        public AcceptCharsetHeader(string value, IEnumerable<QuantifiedHeaderValue> quantifiedHeaderValues) : base(NAME, value, quantifiedHeaderValues)
        {
            ResponseContentEncoding = Encoding.GetEncoding(QuantifiedHeaderValues.First().HeaderValue);
        }

        public override void Visit<T>(IHttpHeaderVisitor<T> v, T arg)
        {
            v.Visit(this, arg);
        }
    }
}
