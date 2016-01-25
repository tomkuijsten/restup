using System.Linq;
using System.Text;

namespace Devkoes.HttpMessage.Plumbing
{
    internal class Constants
    {
        internal static readonly Encoding DefaultHttpEncoding;
        internal static readonly string DefaultHttpCharset;
        internal static readonly string DefaultJSONCharset;
        internal static readonly byte SpaceByte;
        internal static readonly byte CRByte;
        internal static readonly byte LFByte;
        internal static readonly byte ColonByte;

        static Constants()
        {
            DefaultHttpCharset = "iso-8859-1";
            DefaultHttpEncoding = Encoding.GetEncoding(DefaultHttpCharset);

            SpaceByte = Constants.DefaultHttpEncoding.GetBytes(new[] { ' ' }).Single();
            CRByte = Constants.DefaultHttpEncoding.GetBytes(new[] { '\r' }).Single();
            LFByte = Constants.DefaultHttpEncoding.GetBytes(new[] { '\n' }).Single();
            ColonByte = Constants.DefaultHttpEncoding.GetBytes(new[] { ':' }).Single();
        }
    }
}
