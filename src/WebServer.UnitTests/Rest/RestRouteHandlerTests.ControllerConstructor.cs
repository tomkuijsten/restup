using Microsoft.VisualStudio.TestTools.UnitTesting;
using Restup.Webserver.Models.Schemas;
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

        private class ControllerWithPrivateConstructor
        {
            private ControllerWithPrivateConstructor()
            {                
            }
        }

        private class ControllerWithTwoConstructors
        {
            public ControllerWithTwoConstructors()
            {
            }

            public ControllerWithTwoConstructors(string param)
            {
            }
        }

        private class ControllerWithInterfaceConstructor
        {
            private readonly IInterface _interface;

            public ControllerWithInterfaceConstructor(IInterface @interface)
            {
                _interface = @interface;
            }

            public interface IInterface
            { }

            public class Implementation : IInterface
            { }
        }

        [TestMethod]
        public void RegisterController_WithConstructorWithInterfaceParameterAndImplementationIsPassedIn_ThenNoExceptionIsThrown()
        {
            _restRouteHandler.RegisterController<ControllerWithInterfaceConstructor>(new ControllerWithInterfaceConstructor.Implementation());
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void RegisterController_WithConstructorWithInterfaceParameterAndInterfaceIsPassedIn_ThenNoExceptionIsThrown()
        {
            ControllerWithInterfaceConstructor.IInterface @interface = new ControllerWithInterfaceConstructor.Implementation();

            _restRouteHandler.RegisterController<ControllerWithInterfaceConstructor>(@interface);
            Assert.IsTrue(true);
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
        public void RegisterController_WithPrivateConstructer_ThenExceptionIsThrown()
        {
            AssertRegisterControllerThrows<ControllerWithPrivateConstructor>(() => new object[] { "param1" });
        }

        [TestMethod]
        public void RegisterController_WithTwoConstructersAndFuncIsUsed_ThenExceptionIsThrown()
        {
            AssertRegisterControllerThrows<ControllerWithTwoConstructors>(() => new object[] { "param1" });
        }

        [TestMethod]
        public void RegisterController_WithTwoConstructersAndInstantiatedObjectIsUsed_ThenNoExceptionIsThrown()
        {
            _restRouteHandler.RegisterController<ControllerWithTwoConstructors>("param1");
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