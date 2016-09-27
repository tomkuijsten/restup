namespace Restup.Webserver.Http
{
    public interface ICorsConfiguration
    {       
        ICorsConfiguration AddAllowedOrigin(string allowedOrigin);
    }
}