using System.Linq;
using System.Text;

namespace Devkoes.Restup.WebServer.Http
{
    internal class HttpConstants
    {
        internal static readonly Encoding DefaultHttpMessageCharset;
        internal static readonly byte SpaceByte;
        internal static readonly byte CRByte;
        internal static readonly byte LFByte;
        internal static readonly byte ColonByte;



        static HttpConstants()
        {
            // TODO this is not correct, for JSON the default encoding is UTF-8 (http://www.ietf.org/rfc/rfc4627.txt)
            DefaultHttpMessageCharset = Encoding.GetEncoding("iso-8859-1");
            SpaceByte = HttpConstants.DefaultHttpMessageCharset.GetBytes(new[] { ' ' }).Single();
            CRByte = HttpConstants.DefaultHttpMessageCharset.GetBytes(new[] { '\r' }).Single();
            LFByte = HttpConstants.DefaultHttpMessageCharset.GetBytes(new[] { '\n' }).Single();
            ColonByte = HttpConstants.DefaultHttpMessageCharset.GetBytes(new[] { ':' }).Single();
        }
    }
}
