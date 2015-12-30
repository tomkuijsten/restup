namespace Devkoes.Restup.WebServer.Models.Schemas
{
    internal class BadRequestResponse : StatusOnlyResponse
    {
        internal BadRequestResponse() : base(400) { }
    }
}
