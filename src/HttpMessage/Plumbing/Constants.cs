using System.Linq;
using System.Text;

namespace Restup.HttpMessage.Plumbing
{
    internal class Constants
    {
        internal static Encoding DefaultHttpEncoding { get; }

        internal static readonly byte SpaceByte;
        internal static readonly byte CRByte;
        internal static readonly byte LFByte;
        internal static readonly byte ColonByte;

        static Constants()
        {
            DefaultHttpEncoding = Encoding.GetEncoding("iso-8859-1");

            SpaceByte = Constants.DefaultHttpEncoding.GetBytes(new[] { ' ' }).Single();
            CRByte = Constants.DefaultHttpEncoding.GetBytes(new[] { '\r' }).Single();
            LFByte = Constants.DefaultHttpEncoding.GetBytes(new[] { '\n' }).Single();
            ColonByte = Constants.DefaultHttpEncoding.GetBytes(new[] { ':' }).Single();
        }
    }
}
