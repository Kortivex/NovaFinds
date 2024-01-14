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
                if (_log != null) return _log;

                var consoleTarget = new ConsoleTarget();
                var config = new LoggingConfiguration();
                config.AddTarget("console", consoleTarget);
                config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, consoleTarget));

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