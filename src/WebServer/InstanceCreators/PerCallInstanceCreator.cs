using Restup.Webserver.Models.Contracts;
using System;

namespace Restup.Webserver.InstanceCreators
{
    internal class PerCallInstanceCreator : IInstanceCreator
    {
        public object Create(Type instanceType, params object[] args)
        {
            return Activator.CreateInstance(instanceType, args);
        }
    }
}
