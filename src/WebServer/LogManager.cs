using Restup.WebServer.Logging;
using System;
using System.Threading;

namespace Restup.Webserver
{
    public static class LogManager
    {
        private static ILogFactory _logFactory;

        static LogManager()
        {
            _logFactory = new NullLogFactory();
        }

        public static ILogger GetLogger<T>()
        {
            return _logFactory.GetLogger<T>();
        }

        public static ILogger GetLogger(string name)
        {
            return _logFactory.GetLogger(name);
        }

        public static void SetLogFactory(ILogFactory logFactory)
        {
            Interlocked.Exchange(ref _logFactory, logFactory);
        }

        private class NullLogFactory : ILogFactory
        {
            private readonly NullLogger _nullLogger;

            public NullLogFactory()
            {
                _nullLogger = new NullLogger();
            }

            ILogger ILogFactory.GetLogger<T>()
            {
                return _nullLogger;
            }

            ILogger ILogFactory.GetLogger(string name)
            {
                return _nullLogger;
            }

            public void Dispose()
            {
            }
        }

        private class NullLogger : AbstractLogger
        {
            protected override bool IsLogEnabled(LogLevel trace) => false;

            protected override void LogMessage(string message, LogLevel loggingLevel, params object[] args)
            {
            }

            protected override void LogMessage(string message, LogLevel loggingLevel, Exception ex)
            {
            }
        }
    }
}