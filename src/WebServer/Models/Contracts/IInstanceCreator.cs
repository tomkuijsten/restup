using System;
using System.Reflection;

namespace Restup.Webserver.Models.Contracts
{
    interface IInstanceCreator
    {
        object Create(ConstructorInfo instanceType, object[] args);
    }
}
