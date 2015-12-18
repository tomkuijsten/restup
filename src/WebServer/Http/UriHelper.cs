namespace Devkoes.Restup.WebServer.Http
{
    public static class UriHelper
    {
        public static string RemovePreAndPostSlash(string uri)
        {
            if (uri == null)
                return uri;

            return uri.TrimStart('/').TrimEnd('/');
        }
    }
}
