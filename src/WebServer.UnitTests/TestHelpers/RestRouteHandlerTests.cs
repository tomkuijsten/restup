using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Restup.Webserver.Rest;

namespace Restup.Webserver.UnitTests.TestHelpers
{
    public abstract class RestRouteHandlerTests
    {
        protected RestRouteHandler _restRouteHandler;

        [TestInitialize]
        public void Initialize()
        {
            _restRouteHandler = new RestRouteHandler();
        }

        protected void AssertRegisterControllerThrows<T>(params object[] args) where T : class
        {
            Assert.ThrowsException<Exception>(() =>
                _restRouteHandler.RegisterController<T>(args)
            );
        }

        protected void AssertRegisterControllerThrows<T>() where T : class
        {
            Assert.ThrowsException<Exception>(() =>
                _restRouteHandler.RegisterController<T>()
            );
        }
    }
}
