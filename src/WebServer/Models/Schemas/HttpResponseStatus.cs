namespace Devkoes.Restup.WebServer.Models.Schemas
{
    public enum HttpResponseStatus : int
    {
        OK = 200,
        Created = 201,
        NoContent = 204,
        BadRequest = 400,
        NotFound = 404,
        Conflict = 409
    }
}
