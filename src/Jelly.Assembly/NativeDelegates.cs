using System;
using System.Runtime.InteropServices;

namespace Jelly.Assembly
{
    // ──────────────────────────────────────────────────────────────────────────
    /// <summary>
    /// Creates a new engine instance and returns its native handle.
    /// </summary>
    /// <param name="width">Window back-buffer width, in pixels.</param>
    /// <param name="height">Window back-buffer height, in pixels.</param>
    /// <param name="vsync"><c>true</c> to enable vertical sync; otherwise <c>false</c>.</param>
    /// <param name="title">Initial window title.</param>
    /// <param name="apiName">Graphics API name (e.g. <c>"Vulkan"</c>).</param>
    /// <returns>An opaque native handle that represents the engine instance.</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate IntPtr EngineCreateDelegate(
        int width,
        int height,
        bool vsync,
        string title,
        string apiName
    );

    // ──────────────────────────────────────────────────────────────────────────
    /// <summary>
    /// Indicates whether a previously created engine instance is still running.
    /// </summary>
    /// <param name="handle">The native engine handle representing the current engine instance.</param>
    /// <returns><c>true</c> if the engine main loop is active; otherwise <c>false</c>.</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool EngineIsRunningDelegate(IntPtr handle);

    // ──────────────────────────────────────────────────────────────────────────
    /// <summary>
    /// Processes a single iteration of the engine’s event loop.
    /// </summary>
    /// <param name="handle">Native engine handle returned by <see cref="EngineCreateDelegate"/>.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void EnginePollDelegate(IntPtr handle);
    
    // ──────────────────────────────────────────────────────────────────────────
    /// <summary>
    /// Delegate for invoking a single render pass on the native engine.
    /// </summary>
    /// <param name="handle">The native engine handle representing the current engine instance.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void EngineRenderDelegate(IntPtr handle);
    
    // ──────────────────────────────────────────────────────────────────────────
    /// <summary>
    /// Shuts down the engine and releases all associated resources.
    /// </summary>
    /// <param name="handle">The native engine handle representing the current engine instance.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void EngineShutdownDelegate(IntPtr handle);
    
    // ──────────────────────────────────────────────────────────────────────────
    /// <summary>
    /// Native logging function delegate. Used to log messages from managed code to native engine output.
    /// </summary>
    /// <param name="level">Integer log level.</param>
    /// <param name="message">Log message string.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void LoggerLogDelegate(int level, string message);
}