using Devkoes.Restup.WebServer.Models.Contracts;

namespace Devkoes.Restup.WebServer.Rest
{
    internal class RestControllerMethodExecutorFactory
    {
        private IRestMethodExecutor _withoutBodyExecutor;
        private IRestMethodExecutor _withBodyExecutor;

        public RestControllerMethodExecutorFactory()
        {
            _withoutBodyExecutor = new RestControllerMethodExecutor();
            _withBodyExecutor = new RestControllerMethodWithBodyExecutor();

        }
        internal IRestMethodExecutor Create(RestControllerMethodInfo info)
        {
            if (info.HasBodyParameter)
            {
                return _withBodyExecutor;
            }
            else
            {
                return _withoutBodyExecutor;
            }
        }
    }
}
