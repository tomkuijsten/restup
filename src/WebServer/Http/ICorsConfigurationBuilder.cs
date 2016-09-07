namespace Restup.Webserver.Http
{
    public interface ICorsConfigurationBuilder
    {       
        ICorsConfigurationBuilder AddAllowedOrigin(string allowedOrigin);
    }
}