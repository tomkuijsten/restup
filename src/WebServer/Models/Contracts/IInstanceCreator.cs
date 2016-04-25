using System;

namespace Restup.Webserver.Models.Contracts
{
    interface IInstanceCreator
    {
        object Create(Type instanceType, object[] args);
    }
}
