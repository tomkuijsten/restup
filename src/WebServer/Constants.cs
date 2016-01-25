using System.Linq;
using System.Text;

namespace Devkoes.Restup.WebServer.Http
{
    internal class Constants
    {
        internal static string DefaultJSONCharset;
        internal static string DefaultHttpCharset;

        internal static readonly Encoding DefaultHttpMessageCharset;
        internal static readonly byte SpaceByte;
        internal static readonly byte CRByte;
        internal static readonly byte LFByte;
        internal static readonly byte ColonByte;

        static Constants()
        {
            DefaultHttpCharset = "iso-8859-1";
            DefaultJSONCharset = "UTF-8";

            // TODO this is not correct, for JSON the default encoding is UTF-8 (http://www.ietf.org/rfc/rfc4627.txt)
            DefaultHttpMessageCharset = Encoding.GetEncoding(DefaultHttpCharset);
            SpaceByte = Constants.DefaultHttpMessageCharset.GetBytes(new[] { ' ' }).Single();
            CRByte = Constants.DefaultHttpMessageCharset.GetBytes(new[] { '\r' }).Single();
            LFByte = Constants.DefaultHttpMessageCharset.GetBytes(new[] { '\n' }).Single();
            ColonByte = Constants.DefaultHttpMessageCharset.GetBytes(new[] { ':' }).Single();
        }
    }
}
