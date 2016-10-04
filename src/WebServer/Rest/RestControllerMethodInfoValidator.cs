using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Restup.Webserver.Rest
{
    internal class RestControllerMethodInfoValidator
    {
        private readonly UniqueMatchUriAndVerbRestControllerMethodInfoComparer _uniqueMatchUriAndVerbComparer;

        public RestControllerMethodInfoValidator()
        {
            _uniqueMatchUriAndVerbComparer = new UniqueMatchUriAndVerbRestControllerMethodInfoComparer();
        }

        public void Validate<T>(ImmutableArray<RestControllerMethodInfo> existingRestMethodCollection,
            IList<RestControllerMethodInfo> restControllerMethodInfos)
        {
            foreach (var restControllerMethodInfo in restControllerMethodInfos)
            {
                // if the existing rest method collection already contains the rest controller method to be added 
                // or if the rest controller method infos to be added contains more than one rest method info with the same path
                // then throw an exception                 
                if (existingRestMethodCollection.Contains(restControllerMethodInfo, _uniqueMatchUriAndVerbComparer)
                    || restControllerMethodInfos.Count(x => _uniqueMatchUriAndVerbComparer.Equals(x, restControllerMethodInfo)) > 1)
                {
                    throw new Exception($"Can't register route for controller {typeof(T)}, UriFormat with {restControllerMethodInfo.MatchUri} and {restControllerMethodInfo.Verb} since this would cause multiple routes to be registered on the same name.");
                }
            }
        }
    }
}