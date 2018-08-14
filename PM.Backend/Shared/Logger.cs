using System.Runtime.CompilerServices;
using log4net;



namespace PM.Backend.Shared
{
    public class Logger
    {
        public static ILog GetLogger([CallerFilePath]string filename = "")
        {
            return LogManager.GetLogger(filename);
        }
    }
}
