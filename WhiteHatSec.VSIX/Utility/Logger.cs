using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace WhiteHatSec.VSIX.Utility
{
    /// <summary>
    /// Logger class for log4net
    /// </summary>
    public class Logger
    {
        /// <summary>
        ///     Set up the Log file.
        /// </summary>
        /// <param name="logFileName">The log file name.</param>
        public static void Setup(string logFileName)
        {
            Hierarchy logHierarchy = (Hierarchy)LogManager.GetRepository();

            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "[%p] [%d{ISO8601}] [%F:%M:%L]  %m%n";
            patternLayout.ActivateOptions();

            RollingFileAppender rollingFileAppender = new RollingFileAppender();
            rollingFileAppender.AppendToFile = true;
            rollingFileAppender.File = logFileName;
            rollingFileAppender.RollingStyle = RollingFileAppender.RollingMode.Size;
            rollingFileAppender.Layout = patternLayout;
            rollingFileAppender.MaxSizeRollBackups = 5;

            rollingFileAppender.MaximumFileSize = "50MB";

            rollingFileAppender.StaticLogFileName = true;
            rollingFileAppender.ActivateOptions();
            logHierarchy.Root.AddAppender(rollingFileAppender);

            MemoryAppender memoryAppender = new MemoryAppender();
            memoryAppender.ActivateOptions();
            logHierarchy.Root.AddAppender(memoryAppender);

            logHierarchy.Root.Level = Level.Info;
            logHierarchy.Configured = true;
        }
    }
}