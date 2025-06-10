using Jelly.Assembly;

namespace Jelly.Engine;

/// <summary>
/// Provides logging utilities for the engine layer.
/// </summary>
public static class Logger
{
    /// <summary>
    /// Logs an informational message to the native logging system.
    /// </summary>
    /// <param name="message">The message to be logged.</param>
    public static void Info(string message)
    {
        JellyNative.Log((int)LogLevel.Info, message);
    }
}