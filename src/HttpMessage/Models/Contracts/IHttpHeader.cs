namespace Devkoes.HttpMessage.Models.Contracts
{
    public interface IHttpHeader
    {
        string Name { get; set; }
        string Value { get; set; }
    }
}
