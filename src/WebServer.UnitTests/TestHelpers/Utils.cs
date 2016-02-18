using System;

namespace Devkoes.Restup.WebServer.UnitTests.TestHelpers
{
    // helper methods to make it easier to change the creation of classes in the future
    public static class Utils
    {
        public static RestRoutehandler CreateRestRoutehandler<T>() where T : class
        {
            var restRouteHandler = CreateRestRoutehandler();
            restRouteHandler.RegisterController<T>();
            return restRouteHandler;
        }

        public static RestRoutehandler CreateRestRoutehandler<T>(params object[] args) where T : class
        {
            var restRouteHandler = CreateRestRoutehandler();
            restRouteHandler.RegisterController<T>(args);
            return restRouteHandler;
        }

        public static RestRoutehandler CreateRestRoutehandler<T>(Func<object[]> args) where T : class
        {
            var restRouteHandler = CreateRestRoutehandler();
            restRouteHandler.RegisterController<T>(args);
            return restRouteHandler;
        }

        public static RestRoutehandler CreateRestRoutehandler()
        {
            var restRouteHandler = new RestRoutehandler();
            return restRouteHandler;
        }
    }
}