using Microsoft.VisualStudio.TestTools.UnitTesting;
using Restup.Webserver.UnitTests.TestHelpers;

namespace Restup.Webserver.UnitTests.Rest
{
    [TestClass]
    public class RestRouteHandlerTests_ControllerConstructor : RestRouteHandlerTests
    {
        private class ControllerWithOneStringParameter
        {
            public ControllerWithOneStringParameter(string param)
            {
            }
        }

        private class ControllerWithOneStringParameterAndOneIntegerParameter
        {
            public ControllerWithOneStringParameterAndOneIntegerParameter(string param, int param2)
            {
            }
        }

        [TestMethod]
        public void RegisterController_WithConstructerWithAParameterAndNoParamIsPassedIn_ThenExceptionIsThrown()
        {
            AssertRegisterControllerThrows<ControllerWithOneStringParameter>();
        }

        [TestMethod]
        public void RegisterController_WithConstructerWithAStringParameterAndAnIntegerIsPassedIn_ThenExceptionIsThrown()
        {
            AssertRegisterControllerThrows<ControllerWithOneStringParameter>(1);
        }

        [TestMethod]
        public void RegisterController_WithConstructerWithAParameterAndMoreParamsArePassedIn_ThenExceptionIsThrown()
        {
            AssertRegisterControllerThrows<ControllerWithOneStringParameter>("param1", "param2");
        }

        [TestMethod]
        public void RegisterController_WithConstructerWithAParameterAndTheCorrectParamsArePassedIn_ThenNoExceptionIsThrown()
        {
            _restRouteHandler.RegisterController<ControllerWithOneStringParameter>(() => new object[] { "param1" });
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void RegisterController_WithConstructerWithTwoParametersAndLessParamsArePassedIn_ThenExceptionIsThrown()
        {
            AssertRegisterControllerThrows<ControllerWithOneStringParameterAndOneIntegerParameter>("param1");
        }

        [TestMethod]
        public void RegisterController_WithConstructerWithTwoParametersAndTheCorrectParamsArePassedIn_ThenNoExceptionIsThrown()
        {
            _restRouteHandler.RegisterController<ControllerWithOneStringParameterAndOneIntegerParameter>("param1", 1);
            Assert.IsTrue(true);
        }
    }
}