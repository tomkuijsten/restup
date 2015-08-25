using Devkoes.Restup.WebServer.Executors;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Models.Contracts;

namespace Devkoes.Restup.WebServer.Factories
{
    internal class RestMethodExecutorFactory
    {
        private IRestMethodExecutor _withoutBodyExecutor;
        private IRestMethodExecutor _withBodyExecutor;

        public RestMethodExecutorFactory()
        {
            _withoutBodyExecutor = new RestMethodExecutor();
            _withBodyExecutor = new RestMethodWithBodyExecutor();

        }
        internal IRestMethodExecutor Create(RestMethodInfo info)
        {
            if(info.HasBodyParameter)
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
