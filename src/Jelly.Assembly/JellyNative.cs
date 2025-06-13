using System.Runtime.InteropServices;

namespace Jelly.Assembly;

/// <summary>
/// Thin C# wrapper around the native <c>Jelly.dll</c> API.  
/// Loads function pointers once and exposes idiomatic helpers.
/// </summary>
public static partial class JellyNative
{
    /// <summary>Pointer to the loaded native library.</summary>
    private static readonly IntPtr Library;

    static JellyNative()
    {
        Library = NativeLibrary.Load("Jelly.dll");
        
        LoggerLog          = GetDelegate<LoggerLogDelegate>("jellyLogMessage");
        
        EngineInitialize   = GetDelegate<EngineCreateDelegate>("jellyEngineInitialize");
        EngineIsRunning    = GetDelegate<EngineIsRunningDelegate>("jellyEngineIsRunning");
        EnginePoll         = GetDelegate<EnginePollDelegate>("jellyEnginePoll");
        EngineRender       = GetDelegate<EngineRenderDelegate>("jellyEngineRender");
        EngineShutdown     = GetDelegate<EngineShutdownDelegate>("jellyEngineShutdown");
    }

    // ──────────────────────────────────────────────────────────────────────────
    /// <summary>
    /// Retrieves a typed delegate for a native symbol.
    /// </summary>
    /// <typeparam name="T">Delegate type matching the native function signature.</typeparam>
    /// <param name="name">Symbol name exported by the DLL.</param>
    private static T GetDelegate<T>(string name) where T : Delegate
    {
        var symbol = NativeLibrary.GetExport(Library, name);
        return Marshal.GetDelegateForFunctionPointer<T>(symbol);
    }
}