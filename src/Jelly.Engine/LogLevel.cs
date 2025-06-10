namespace Jelly.Engine;

/// <summary>
/// Represents the severity or purpose of a log message.
/// </summary>
internal enum LogLevel
{
    /// <summary>Standard information for general output.</summary>
    Info,

    /// <summary>Highlighted information, e.g., successful steps or key events.</summary>
    Highlight,

    /// <summary>Non-fatal issues or recoverable problems.</summary>
    Warning,

    /// <summary>Serious issues that require developer attention.</summary>
    Error
}