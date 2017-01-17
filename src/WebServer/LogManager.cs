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
            _logFactory = new DebugLogFactory();
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

        private class DebugLogFactory : ILogFactory
        {
            private ILogger _debugLogger;

            public DebugLogFactory()
            {
                _debugLogger = new DebugLogger();
            }

            public void Dispose()
            {
                _debugLogger = null;
            }

            ILogger ILogFactory.GetLogger(string name)
            {
                return _debugLogger;
            }

            ILogger ILogFactory.GetLogger<T>()
            {
                return _debugLogger;
            }
        }

        public class DebugLogger : AbstractLogger
        {
            protected override bool IsLogEnabled(LogLevel trace)
            {
                // Ignore level, log everything
                return true;
            }

            protected override void LogMessage(string message, LogLevel loggingLevel, Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"{DateTime.UtcNow.ToString("dd-MM-yyyy hh:mm:ss.fff")} {loggingLevel}: {message}. Exception: {Environment.NewLine} {ex}");
            }

            protected override void LogMessage(string message, LogLevel loggingLevel, params object[] args)
            {
                System.Diagnostics.Debug.WriteLine($"{DateTime.UtcNow.ToString("dd-MM-yyyy hh:mm:ss.fff")} {loggingLevel}: {(string.Format(message, args))}");
            }
        }
    }
}