using System;

namespace Restup.WebServer.Logging
{
    public interface ILogFactory : IDisposable
    {
        ILogger GetLogger<T>();
        ILogger GetLogger(string name);
    }
}
