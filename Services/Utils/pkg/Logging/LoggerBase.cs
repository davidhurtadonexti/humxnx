using log4net;
using pkg.Interfaces;

namespace pkg.Logging
{
    /// <summary>
    /// Base to print logs
    /// </summary>
    public class LoggerBase : ILoggerRuntime
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILog _logger;
        private readonly ILog _loggerGeneralLog;

        public LoggerBase()
        {
            _logger = LogManager.GetLogger(typeof(LoggerBase));
            _loggerGeneralLog = LogManager.GetLogger("GeneralLog");
        }
        public void LogError(string message)
        {
            _logger.Error(message);
        }
        public void LogInfo(string message)
        {
            _logger.Info(message);
        }
        public void LogDebug(string message)
        {
            _logger.Debug(message);
        }
        public void LogWarn(string message)
        {
            _logger.Warn(message);
        }
    }
}

