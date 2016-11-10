using System.Diagnostics;

namespace Telegram.Net.Core
{
    public interface ILogger
    {
        void WriteLogLine(string logMessage);
    }

    public class DefaultDebugLogger : ILogger
    {
        public void WriteLogLine(string logMessage)
        {
            Debug.WriteLine(logMessage);
        }
    }

    public static class Logger
    {
        public static ILogger logger = new DefaultDebugLogger();
        
        public static void WriteLine(string logMessage)
        {
            logger.WriteLogLine(logMessage);
        }
    }
}
