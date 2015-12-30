using Devkoes.Restup.WebServer.Models.Contracts;
using System;

namespace Devkoes.Restup.WebServer.InstanceCreators
{
    internal class PerCallInstanceCreator : IInstanceCreator
    {
        public object Create(Type instanceType, params object[] args)
        {
            return Activator.CreateInstance(instanceType, args);
        }
    }
}
