using Restup.Webserver.Rest;
using Restup.Webserver.UnitTests.TestHelpers;

namespace Restup.Webserver.UnitTests.Rest
{
    public class FluentRestRouteHandlerTests : FluentHttpServerTests<FluentRestRouteHandlerTests>
    {
        public FluentRestRouteHandlerTests ControllersIsRegistered<T>(string urlPrefix, params object[] arguments) where T : class
        {
            var restRouteHandler = new RestRouteHandler();
            restRouteHandler.RegisterController<T>(arguments);
            RegisterRouteHandler(urlPrefix, restRouteHandler);

            return this;
        }
    }
}