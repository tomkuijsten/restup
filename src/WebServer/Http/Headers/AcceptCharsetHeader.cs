using Devkoes.Restup.WebServer.Models.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Devkoes.Restup.WebServer.Http.Headers
{
    internal class AcceptCharsetHeader : HttpMultiQuantifiedHeaderBase
    {
        internal static string NAME = "Accept-Charset";

        public Encoding ResponseContentEncoding { get; set; }

        public AcceptCharsetHeader(string value, IEnumerable<QuantifiedHeaderValue> quantifiedHeaderValues)
            : base(NAME, value, quantifiedHeaderValues)
        {
            ResponseContentEncoding = Encoding.GetEncoding(QuantifiedHeaderValues.First().HeaderValue);
        }

        public override void Visit<T>(IHttpHeaderVisitor<T> v, T arg)
        {
            v.Visit(this, arg);
        }
    }
}
