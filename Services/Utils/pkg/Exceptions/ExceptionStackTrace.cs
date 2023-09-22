using System;
using System.Diagnostics;
using pkg.Interfaces;

namespace pkg.Exceptions
{
	public class ExceptionStackTrace : Exception
    {
        public string? File { get; }
        public string? Package { get; }
        public string? FunctionName { get; }
        public int? LineNumber { get; }


        public ExceptionStackTrace(string message, StackTrace stackTrace, ILoggerRuntime logger) : base(message)
        {

            // Get the top stack frame
            var frame = stackTrace.GetFrame(0);

            File = frame?.GetFileName();
            Package = frame?.GetMethod()?.DeclaringType?.Namespace;
            FunctionName = frame?.GetMethod()?.Name;
            LineNumber = frame?.GetFileLineNumber();

            // Print the variable to the console
            PrintVariables(message, logger);
        }
        public ExceptionStackTrace(string message, Exception innerException, StackTrace stackTrace, ILoggerRuntime logger) : base(message, innerException)
        {

            // Get the top stack frame
            var frame = stackTrace.GetFrame(0);

            File = frame?.GetFileName();
            Package = frame?.GetMethod()?.DeclaringType?.Namespace;
            FunctionName = frame?.GetMethod()?.Name;
            LineNumber = frame?.GetFileLineNumber();

            // Print the variable to the console
            PrintVariables(message, logger);
        }
        private void PrintVariables(string message, ILoggerRuntime logger)
        {
            logger.LogError($"Location: {Package}.{FunctionName} ({File}:{LineNumber})\nException: {message}");
            Console.WriteLine($"Location: {Package}.{FunctionName} ({File}:{LineNumber})");
        }
    }
}

