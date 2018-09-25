using System;

namespace PM.Shared
{
    public class EventLog
    {
        public enum LogEventType
        {
            Login,
            Error,
            RenameFolder,
            HideFolder,
            NewFolder
        }
        public DateTime LogTimestamp { get; set; }
        public string LogUser { get; set; }
        public string LogType { get; set; }
        public string LogMessage { get; set; }

        public static void AddEventLog(LogEventType eventType, string message)
        {
            DBHelper.Instance.AddEventLog(eventType, message);
        }
    }
}
