namespace Devkoes.HttpMessage.ServerResponseParsers
{
    internal class HttpServerResponseParser
    {
        internal static HttpServerResponseParser Default { get; }

        static HttpServerResponseParser()
        {
            Default = new HttpServerResponseParser();
        }

        public string ConvertToString(HttpServerResponse response)
        {
            return null;
        }

        public byte[] ConvertToBytes(HttpServerResponse response)
        {
            return null;
        }
    }
}
