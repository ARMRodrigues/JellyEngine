using System.Runtime.InteropServices;

namespace JellyAssembly.GLFW;

public partial class GLFW
{
    /// <summary>
    /// Returns the primary monitor.
    /// </summary>
    /// <returns>The primary monitor, or <see cref="IntPtr.Zero"/> if no monitors were found or an error occurred.</returns>
    [LibraryImport(NativeHelperName.GLFWLibraryName, EntryPoint = "glfwGetPrimaryMonitor")]
    private static partial IntPtr glfwGetPrimaryMonitor();

    /// <summary>
    /// Gets the primary monitor.
    /// </summary>
    /// <returns>The primary monitor, or <see cref="IntPtr.Zero"/> if no monitors were found or an error occurred.</returns>
    public static IntPtr GetPrimaryMonitor()
    {
        // Call the native GLFW function to get the primary monitor.
        return glfwGetPrimaryMonitor();
    }

    /// <summary>
    /// Returns the current video mode of the specified monitor.
    /// </summary>
    /// <param name="monitor">The monitor to query.</param>
    /// <returns>A pointer to the video mode, or <see cref="IntPtr.Zero"/> if an error occurred.</returns>
    [LibraryImport(NativeHelperName.GLFWLibraryName, EntryPoint = "glfwGetVideoMode")]
    private static partial IntPtr glfwGetVideoMode(IntPtr monitor);

    /// <summary>
    /// Gets the current video mode of the specified monitor.
    /// </summary>
    /// <param name="monitor">The monitor to query.</param>
    /// <returns>The video mode of the monitor.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the video mode could not be retrieved.</exception>
    public static GLFWvidmode GetVideoMode(IntPtr monitor)
    {
        // Call the native GLFW function to get the video mode.
        IntPtr videoModePtr = glfwGetVideoMode(monitor);

        if (videoModePtr == IntPtr.Zero)
        {
            throw new InvalidOperationException("Failed to retrieve the video mode.");
        }

        // Marshal the pointer to the GLFWvidmode struct.
        return Marshal.PtrToStructure<GLFWvidmode>(videoModePtr);
    }
}
