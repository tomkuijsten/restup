namespace Restup.HttpMessage.Headers.Response
{
    public class CloseConnectionHeader : HttpHeaderBase
    {
        internal static string NAME = "Connection";

        public CloseConnectionHeader() : base(NAME, "close") { }
    }
}
