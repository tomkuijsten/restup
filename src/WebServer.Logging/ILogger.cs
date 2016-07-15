using System;

namespace Restup.WebServer.Logging
{
    public interface ILogger
    {
        bool IsLogEnabled();
        void Trace(string message, Exception ex);
        void Trace(string message, params object[] args);

        bool IsDebugEnabled();
        void Debug(string message, Exception ex);
        void Debug(string message, params object[] args);

        bool IsInfoEnabled();
        void Info(string message, Exception ex);
        void Info(string message, params object[] args);

        bool IsWarnEnabled();
        void Warn(string message, Exception ex);
        void Warn(string message, params object[] args);

        bool IsErrorEnabled();
        void Error(string message, Exception ex);
        void Error(string message, params object[] args);

        bool IsFatalEnabled();
        void Fatal(string message, Exception ex);
        void Fatal(string message, params object[] args);
    }
}