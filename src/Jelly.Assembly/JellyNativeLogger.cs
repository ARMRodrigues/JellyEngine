namespace Jelly.Assembly;

public static partial class JellyNative
{
    /// <summary>
    /// Delegate bound to the native log function.
    /// </summary>
    private static readonly LoggerLogDelegate LoggerLog;

    /// <summary>
    /// Sends a log message to the native engine with the specified log level.
    /// </summary>
    /// <param name="logLevel">Log level as an integer (see <see cref="Jelly.Engine.LogLevel"/>).</param>
    /// <param name="message">The log message.</param>
    public static void Log(int logLevel, string message)
        => LoggerLog(logLevel, message);
}