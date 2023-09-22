namespace pkg.Interfaces
{
	public interface ILoggerRuntime
	{
        void LogError(string message);
        void LogInfo(string message);
        void LogDebug(string message);
        void LogWarn(string message);
    }
}

