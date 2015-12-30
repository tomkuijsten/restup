using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Contracts;
using Devkoes.Restup.WebServer.Models.Schemas;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Devkoes.Restup.WebServer.InstanceCreators
{
    internal class InstanceCreatorCache
    {
        internal static InstanceCreatorCache Default { get; }

        private Dictionary<Type, IInstanceCreator> _cache;

        static InstanceCreatorCache()
        {
            Default = new InstanceCreatorCache();
        }

        internal InstanceCreatorCache()
        {
            _cache = new Dictionary<Type, IInstanceCreator>();
        }

        internal IInstanceCreator GetCreator(Type restController)
        {
            CacheCreator(restController);
            return _cache[restController];
        }

        internal void CacheCreator(Type restController)
        {
            if (!_cache.ContainsKey(restController))
            {
                var restControllerAtt = restController.GetTypeInfo().GetCustomAttribute<RestControllerAttribute>();
                InstanceCreationType t = restControllerAtt != null ? restControllerAtt.InstanceCreationType : InstanceCreationType.Singleton;

                if (t == InstanceCreationType.PerCall)
                {
                    _cache[restController] = new PerCallInstanceCreator();
                }
                else
                {
                    _cache[restController] = new SingletonInstanceCreator();
                }
            }
        }
    }
}
