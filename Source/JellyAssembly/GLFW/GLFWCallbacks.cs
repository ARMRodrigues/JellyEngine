namespace JellyAssembly.GLFW;

using System;
using System.Runtime.InteropServices;

public static class GLFWCallbacks
{
    /// <summary>
    /// Delegate type for framebuffer size callback.
    /// </summary>
    /// <param name="window">The handle to the window that was resized.</param>
    /// <param name="width">The new width of the framebuffer.</param>
    /// <param name="height">The new height of the framebuffer.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FramebufferSizeCallback(IntPtr window, int width, int height);

    /// <summary>
    /// Stores the user-defined framebuffer size callback.
    /// </summary>
    public static event Action<int, int>? OnFramebufferSizeChanged;

    /// <summary>
    /// Internal delegate for GLFW to use.
    /// </summary>
    private static readonly FramebufferSizeCallback InternalFramebufferSizeCallback = FramebufferSizeCallbackInvoker;

    /// <summary>
    /// Gets the function pointer for the GLFW framebuffer size callback.
    /// </summary>
    public static IntPtr FramebufferSizeCallbackPointer => Marshal.GetFunctionPointerForDelegate(InternalFramebufferSizeCallback);

    /// <summary>
    /// Invokes the registered framebuffer size callback with the provided parameters.
    /// </summary>
    private static void FramebufferSizeCallbackInvoker(IntPtr window, int width, int height)
    {
        OnFramebufferSizeChanged?.Invoke(width, height);
    }
}
