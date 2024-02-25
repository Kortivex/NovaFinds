// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Logger.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Logger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.IFR.Logger
{
    using NLog;
    using NLog.Config;
    using NLog.Targets;
    using System.Text;

    /// <summary>
    /// NLog class.
    /// </summary>
    public static class Logger
    {
        private static ILogger? _log;

        private static ILogger Log
        {
            get
            {
                var path = Directory.GetCurrentDirectory() + "/bin/Debug/net8.0";
                if (_log != null) return _log;
                var config = new LoggingConfiguration();

                // File output.
                var fileTarget = new FileTarget
                {
                    Name = "DefaultFileTarget",
                    FileName = path + "/logs/logfile.log",
                    Layout = "${longdate}|${level}|${message}|${exception}",
                    ConcurrentWrites = true,
                    Encoding = Encoding.UTF8
                };
                config.AddTarget("file", fileTarget);
                config.AddRuleForOneLevel(LogLevel.Error, fileTarget);
                config.AddRuleForOneLevel(LogLevel.Info, fileTarget);
                config.AddRuleForOneLevel(LogLevel.Warn, fileTarget);
                config.AddRuleForOneLevel(LogLevel.Fatal, fileTarget);
                config.AddRuleForOneLevel(LogLevel.Trace, fileTarget);

                // Console output.
                var consoleTarget = new ConsoleTarget
                {
                    Name = "DefaultConsole",
                    Layout = "${longdate}|${level}|${message}|${exception}",
                    DetectConsoleAvailable = true,
                    Encoding = Encoding.UTF8
                };
                config.AddTarget("console", consoleTarget);
                config.AddRuleForOneLevel(LogLevel.Debug, consoleTarget);
                config.AddRuleForOneLevel(LogLevel.Info, consoleTarget);
                config.AddRuleForOneLevel(LogLevel.Warn, consoleTarget);
                config.AddRuleForOneLevel(LogLevel.Fatal, consoleTarget);
                config.AddRuleForOneLevel(LogLevel.Trace, consoleTarget);

                LogManager.Configuration = config;
                _log = LogManager.GetCurrentClassLogger();
                return _log;
            }
        }

        /// <summary>
        /// Write a Fatal error in the log
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="ex">Exception if that exist</param>
        public static void Fatal(string message, Exception? ex = null)
        {
            Log.Fatal(ex, message);
        }

        /// <summary>
        /// Write a Warning in the log
        /// </summary>
        /// <param name="message">Warning message</param>
        /// <param name="ex">Exception if that exist</param>
        public static void Warn(string message, Exception? ex = null)
        {
            Log.Warn(ex, message);
        }

        /// <summary>
        /// Write a Error in the log
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="ex">Exception if that exist</param>
        public static void Error(string message, Exception? ex = null)
        {
            Log.Error(ex, message);
        }

        /// <summary>
        /// Write a Information in the log
        /// </summary>
        /// <param name="message">Info. message</param>
        /// <param name="ex">Exception if that exist</param>
        public static void Info(string message, Exception? ex = null)
        {
            Log.Info(ex, message);
        }

        /// <summary>
        /// Write a Debug in the log
        /// </summary>
        /// <param name="message">Debug message</param>
        /// <param name="ex">Exception if that exist</param>
        public static void Debug(string message, Exception? ex = null)
        {
            Log.Debug(ex, message);
        }
    }
}