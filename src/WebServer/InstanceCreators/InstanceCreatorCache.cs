using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Restup.Webserver.InstanceCreators
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
