using Jelly.Assembly;

namespace Jelly.Engine;

/// <summary>
/// High-level façade that boots, runs, and shuts down a native Jelly engine instance.
/// </summary>
public class JellyApplication
{
    /// <summary>
    /// Opaque native handle returned by the engine bootstrapper.
    /// </summary>
    private readonly IntPtr _jellyHandle;

    // ──────────────────────────────────────────────────────────────────────────
    /// <summary>
    /// Creates a new <see cref="JellyApplication"/> and boots the native engine.
    /// </summary>
    /// <param name="windowSettings">Initial window and runtime parameters.</param>
    public JellyApplication(WindowSettings windowSettings)
    {
        _jellyHandle = JellyNative.Initialize(
            windowSettings.Width,
            windowSettings.Height,
            windowSettings.Vsync,
            windowSettings.Title,
            "Vulkan"); // Currently hard-wired to Vulkan; expose later if needed.

        if (_jellyHandle == IntPtr.Zero)
        {
            Environment.Exit(1);
        }
    }

    // ──────────────────────────────────────────────────────────────────────────
    /// <summary>
    /// Enters the main loop and blocks until <see cref="Stop"/> is called
    /// or the window is closed.
    /// </summary>
    public void Play() => Lifecycle();

    // ──────────────────────────────────────────────────────────────────────────
    /// <summary>
    /// Runs the engine’s event/render loop until the native side reports it should exit.
    /// </summary>
    private void Lifecycle()
    {
        while (JellyNative.IsRunning(_jellyHandle))
            JellyNative.Poll(_jellyHandle);
    }

    // ──────────────────────────────────────────────────────────────────────────
    /// <summary>
    /// Terminates the engine and releases all native resources.
    /// Safe to call multiple times.
    /// </summary>
    public void Stop() => JellyNative.Shutdown(_jellyHandle);
}