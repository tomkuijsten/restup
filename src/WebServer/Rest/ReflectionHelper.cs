using System;
using System.Linq;
using System.Reflection;

namespace Restup.Webserver.Rest
{
    internal static class ReflectionHelper
    {
        internal static bool TryFindMatchingConstructor<T>(Func<object[]> constructorArgs, out ConstructorInfo foundConstructor)
        {
            var args = constructorArgs();
            foreach (var constructorInfo in typeof(T).GetConstructors())
            {
                var parameters = constructorInfo.GetParameters();
                if (args.Length != parameters.Length)
                    continue;

                var argsTypes = args.Select(x => x.GetType());
                var parameterTypes = parameters.Select(x => x.ParameterType);
                if (!argsTypes.SequenceEqual(parameterTypes))
                    continue;

                foundConstructor = constructorInfo;
                return true;
            }
            foundConstructor = null;
            return false;
        }
    }
}