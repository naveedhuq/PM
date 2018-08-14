using System;
using log4net;

namespace PM.Shared
{
    public class Logger : ILogger
    {
        ILog log = null;

        public Logger(Type t)
        {
            log = log4net.LogManager.GetLogger(t);
        }

        public void Debug(string message)
        {
            log.Debug(message);
        }

        public void Error(string message, Exception ex = null)
        {
            log.Error(message, ex);
        }

        public void Fatal(string message, Exception ex = null)
        {
            log.Fatal(message, ex);
        }

        public void Info(string message)
        {
            log.Info(message);
        }

        public void Warn(string message)
        {
            log.Warn(message);
        }
    }
}
