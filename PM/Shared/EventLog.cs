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
            NewFolder,
            DeleteFolder,
            DocumentImport,
            DocumentUpdate,
            DocumentDelete
        }
        public DateTime LogTimestamp { get; set; }
        public string LogUser { get; set; }
        public string LogType { get; set; }
        public string LogMessage { get; set; }

        public static void AddEventLog(LogEventType eventType, string message)
        {
            DBHelper.Instance.AddEventLog(eventType, message);
        }

        public static void AddDocumentActivityLog(LogEventType eventType, string customerName, string documentFileName, string folderName)
        {
            DBHelper.Instance.AddDocumentActivityLog(eventType, customerName, documentFileName, folderName);
        }
    }
}
