using System;

namespace Devkoes.Restup.WebServer.Models.Contracts
{
    public interface IInstanceCreator
    {
        object Create(Type instanceType, params object[] args);
    }
}
