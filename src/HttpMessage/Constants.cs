using System.Linq;
using System.Text;

namespace Devkoes.HttpMessage
{
    internal class Constants
    {
        internal static readonly Encoding DefaultHttpMessageCharset;
        internal static readonly byte SpaceByte;
        internal static readonly byte CRByte;
        internal static readonly byte LFByte;
        internal static readonly byte ColonByte;

        static Constants()
        {
            // TODO this is not correct, for JSON the default encoding is UTF-8 (http://www.ietf.org/rfc/rfc4627.txt)
            DefaultHttpMessageCharset = Encoding.GetEncoding("iso-8859-1");
            SpaceByte = Constants.DefaultHttpMessageCharset.GetBytes(new[] { ' ' }).Single();
            CRByte = Constants.DefaultHttpMessageCharset.GetBytes(new[] { '\r' }).Single();
            LFByte = Constants.DefaultHttpMessageCharset.GetBytes(new[] { '\n' }).Single();
            ColonByte = Constants.DefaultHttpMessageCharset.GetBytes(new[] { ':' }).Single();
        }
    }
}
