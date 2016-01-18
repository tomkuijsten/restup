using System;

namespace Devkoes.Restup.WebServer.Models.Contracts
{
    interface IInstanceCreator
    {
        object Create(Type instanceType, object[] args);
    }
}
