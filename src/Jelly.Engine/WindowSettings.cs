using System.Runtime.InteropServices;

namespace Jelly.Engine;

/// <summary>
/// Plain-old data container for window creation parameters.
/// </summary>
public struct WindowSettings
{
    /// <summary>Back-buffer width in pixels.</summary>
    public int Width;

    /// <summary>Back-buffer height in pixels.</summary>
    public int Height;

    /// <summary>
    /// Enables vertical synchronization (sent to native code as a single byte).
    /// </summary>
    [MarshalAs(UnmanagedType.I1)]
    public bool Vsync;

    /// <summary>Initial window title.</summary>
    public string Title;
}