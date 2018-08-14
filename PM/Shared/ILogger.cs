using System;


namespace PM.Shared
{
    public interface ILogger
    {
        void Info(string message);
        void Debug(string message);
        void Fatal(string message, Exception ex = null);
        void Warn(string message);
        void Error(string message, Exception ex = null);
    }
}
