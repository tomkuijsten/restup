namespace Devkoes.Restup.WebServer.Models.Contracts
{
    public interface IInstanceCreator<T> where T:class
    {
        T Create(params object[] args);
    }
}
