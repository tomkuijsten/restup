using Restup.Webserver.Models.Contracts;
using System;
using System.Reflection;

namespace Restup.Webserver.InstanceCreators
{
    internal class PerCallInstanceCreator : IInstanceCreator
    {
        public object Create(ConstructorInfo instanceType, object[] args)
        {
            return instanceType.Invoke(args);
        }
    }
}
