namespace Devkoes.Restup.WebServer.Models.Schemas
{
    public class BadRequestResponse : StatusOnlyResponse
    {
        public BadRequestResponse() : base(400) { }
    }
}
