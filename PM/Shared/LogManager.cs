using System;
using System.IO;


namespace PM.Shared
{
    public class LogManager
    {
        static LogManager()
        {
            string log4netconfigFile = Properties.Settings.Default.LoggerXMLConfigFile;
            FileInfo f = new FileInfo(log4netconfigFile);
            log4net.Config.XmlConfigurator.Configure(f);
        }

        public static ILogger GetLogger(Type t)
        {
            Logger logger = new Logger(t);
            return logger;
        }
    }
}
