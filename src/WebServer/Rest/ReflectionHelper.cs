using System;
using System.Linq;
using System.Reflection;

namespace Restup.Webserver.Rest
{
    internal static class ReflectionHelper
    {
        internal static bool TryFindMatchingConstructor<T>(object[] args, out ConstructorInfo foundConstructor)
        {
            foreach (var constructorInfo in typeof(T).GetConstructors())
            {
                var parameters = constructorInfo.GetParameters();
                if (args.Length != parameters.Length)
                    continue;

                var argsTypes = args.Select(x => x.GetType()).ToList();
                var parameterTypes = parameters.Select(x => x.ParameterType).ToList();

                for (int i = 0; i < parameterTypes.Count; i++)
                {
                    var param = parameterTypes[i];
                    var arg = argsTypes[i];

                    if (param != arg && !param.IsAssignableFrom(arg))
                    {
                        foundConstructor = null;
                        return false;
                    }
                }

                foundConstructor = constructorInfo;
                return true;
            }
            foundConstructor = null;
            return false;
        }
    }
}