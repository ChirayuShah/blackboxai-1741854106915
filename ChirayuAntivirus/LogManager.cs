using System;
using System.IO;
using System.Text;
using System.Threading;

namespace ChirayuAntivirus
{
    public static class LogManager
    {
        private static readonly string LogFilePath = "ChirayuAntivirus.log";
        private static readonly object lockObject = new object();

        public static void LogInfo(string message)
        {
            WriteToLog("INFO", message);
        }

        public static void LogWarning(string message)
        {
            WriteToLog("WARNING", message);
        }

        public static void LogError(string message, Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(message);
            sb.AppendLine($"Exception: {ex.Message}");
            sb.AppendLine($"Stack Trace: {ex.StackTrace}");

            if (ex.InnerException != null)
            {
                sb.AppendLine($"Inner Exception: {ex.InnerException.Message}");
                sb.AppendLine($"Inner Stack Trace: {ex.InnerException.StackTrace}");
            }

            WriteToLog("ERROR", sb.ToString());
        }

        private static void WriteToLog(string level, string message)
        {
            try
            {
                lock (lockObject)
                {
                    string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] [{Thread.CurrentThread.ManagedThreadId}] {message}";
                    
                    // Ensure directory exists
                    string directory = Path.GetDirectoryName(LogFilePath);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Write to log file
                    File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                // If logging fails, write to console as last resort
                Console.WriteLine($"Failed to write to log file: {ex.Message}");
                Console.WriteLine($"Original message: {message}");
            }
        }

        public static void ClearLog()
        {
            try
            {
                lock (lockObject)
                {
                    if (File.Exists(LogFilePath))
                    {
                        File.Delete(LogFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to clear log file: {ex.Message}");
            }
        }

        public static string[] GetRecentLogs(int count = 100)
        {
            try
            {
                lock (lockObject)
                {
                    if (File.Exists(LogFilePath))
                    {
                        var lines = File.ReadAllLines(LogFilePath);
                        return lines.Length <= count ? lines : lines.Skip(lines.Length - count).ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to read log file: {ex.Message}");
            }
            return new string[0];
        }
    }
}
