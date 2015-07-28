using Devkoes.Restup.WebServer.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;

namespace Devkoes.Restup.WebServer.InstanceCreators
{
    internal static class InstanceCreatorCache
    {
        private static Dictionary<Type, IInstanceCreator> _cache;

        static InstanceCreatorCache()
        {
            _cache = new Dictionary<Type, IInstanceCreator>();
        }

        internal static IInstanceCreator GetCreator(Type restController)
        {
            CacheIfNotAvailable(restController);
            return _cache[restController];
        }

        private static void CacheIfNotAvailable(Type restController)
        {
            if(!_cache.ContainsKey(restController))
            {
                var restControllerAtt = restController.GetTypeInfo().GetCustomAttribute<RestControllerAttribute>();
                InstanceCreationType t = restControllerAtt != null ? restControllerAtt.InstanceCreationType : InstanceCreationType.Singleton;

                if(t == InstanceCreationType.PerCall)
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
