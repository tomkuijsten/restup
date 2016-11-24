using System;
using System.Reflection;
using Restup.Webserver.Models.Contracts;

namespace Restup.Webserver.InstanceCreators
{
    internal class SingletonInstanceCreator : IInstanceCreator
    {
        private object _instance;
        private readonly object _instanceLock = new object();

        public object Create(ConstructorInfo instanceType, object[] args)
        {
            CacheInstance(instanceType, args);

            return _instance;
        }

        private void CacheInstance(ConstructorInfo instanceType, object[] args)
        {
            if (_instance == null)
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = instanceType.Invoke(args);
                    }
                }
            }
        }
    }
}
