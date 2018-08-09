using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;

public class LogHelper
{
    public static void LogFile(LogType logType, string subject)
    {
        LogHelper.LogFile(logType, subject, string.Empty, string.Empty);
    }

    public static void LogFile(LogType logType, string subject, Exception ex, string fileName = "", string errorType = "")
    {
        LogHelper.LogFile(logType, errorType, subject + ex.Message, ex.StackTrace);
    }

    private static void LogFile(LogType logType, string subject, string description, string exception)
    {
        try
        {
            StringBuilder sbContent = new StringBuilder();

            if (subject != string.Empty)
                sbContent.AppendFormat("{0} {1}: {2}\r\n", DateTime.Now.ToString(), logType.ToString(), subject);

            if (description != string.Empty)
                sbContent.AppendFormat("{0} {1}\r\n", DateTime.Now.ToString(), description);

            if (exception != string.Empty)
                sbContent.AppendFormat("{0} {1} \r\n", DateTime.Now.ToString(), exception);

            sbContent.AppendLine();

            LogIntoFile(logType, sbContent.ToString());
        }
        catch
        {
        }
    }

    private static void LogIntoFile(LogType logType, string message)
    {
        StreamWriter eventFile = null;
        try
        {
            DateTime now = DateTime.Now;

            string logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log", now.ToString("yyyy-MM").ToString());

            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }

            string logSuffix = (logType == LogType.Error ? "_debug.txt" : "_action.txt");
            string logPath = Path.Combine(logFolder, now.ToString("yyyyMMdd").ToString() + logSuffix);

            eventFile = new StreamWriter(logPath, true);
            eventFile.Write(message);
            eventFile.Flush();
        }
        catch
        {
        }
        finally
        {
            if (eventFile != null) { eventFile.Close(); eventFile = null; }
        }
    }
}

public enum LogType
{
    Info,
    Warning,
    Error,
    Record,
}