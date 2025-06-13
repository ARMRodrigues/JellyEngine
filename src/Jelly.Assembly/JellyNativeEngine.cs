namespace Jelly.Assembly;

public partial class JellyNative
{
    private static readonly EngineCreateDelegate EngineInitialize;
    /// <summary>
    /// Initialize a new engine instance.
    /// </summary>
    public static IntPtr Initialize(int width, int height, bool vsync, string title, string apiName)
        => EngineInitialize(width, height, vsync, title, apiName);
    
    // ──────────────────────────────────────────────────────────────────────────
    private static readonly EngineIsRunningDelegate EngineIsRunning;
    /// <summary>
    /// Returns <c>true</c> while the engine’s main loop should continue running.
    /// </summary>
    public static bool IsRunning(IntPtr handle)
        => EngineIsRunning(handle);
    
    // ──────────────────────────────────────────────────────────────────────────
    private static readonly EnginePollDelegate EnginePoll;
    /// <summary>
    /// Polls events and advances one frame of the native engine.
    /// </summary>
    public static void Poll(IntPtr handle)
        => EnginePoll(handle);
    
    // ──────────────────────────────────────────────────────────────────────────
    private static readonly EngineRenderDelegate EngineRender;
    /// <summary>
    /// Polls events and advances one frame of the native engine.
    /// </summary>
    public static void Render(IntPtr handle)
        => EngineRender(handle);
    
    // ──────────────────────────────────────────────────────────────────────────
    private static readonly EngineShutdownDelegate EngineShutdown;
    /// <summary>
    /// Shuts down the engine and releases all associated native resources.
    /// </summary>
    /// <param name="handle">The native engine handle previously returned by <see cref="Initialize"/>.</param>
    public static void Shutdown(IntPtr handle)
        => EngineShutdown(handle);
}