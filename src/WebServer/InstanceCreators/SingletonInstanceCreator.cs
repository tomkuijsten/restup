using Restup.Webserver.Models.Contracts;
using System;

namespace Restup.Webserver.InstanceCreators
{
    internal class SingletonInstanceCreator : IInstanceCreator
    {
        private object _instance;
        private object _instanceLock = new object();

        public object Create(Type instanceType, params object[] args)
        {
            CacheInstance(instanceType, args);

            return _instance;
        }

        private void CacheInstance(Type instanceType, object[] args)
        {
            if (_instance == null)
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = Activator.CreateInstance(instanceType, args);
                    }
                }
            }
        }
    }
}
