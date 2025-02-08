using System.Runtime.InteropServices;

namespace JellyAssembly.GLFW;

public partial class GLFW
{
    /// <summary>
    /// Sets the key callback of the specified window, which will be called when a key is pressed, repeated, or released.
    /// </summary>
    /// <param name="window">The window whose callback to set.</param>
    /// <param name="callback">The new key callback, or null to remove the currently set callback.</param>
    /// <returns>The previously set callback, or null if no callback was set.</returns>
    [LibraryImport(NativeHelperName.GLFWLibraryName, EntryPoint = "glfwSetKeyCallback")]
    private static partial IntPtr glfwSetKeyCallback(IntPtr window, KeyCallback callback);

    /// <summary>
    /// Delegate for the key callback function.
    /// </summary>
    /// <param name="window">The window that received the event.</param>
    /// <param name="key">The keyboard key that was pressed or released.</param>
    /// <param name="scancode">The system-specific scancode of the key.</param>
    /// <param name="action">GLFW_PRESS, GLFW_RELEASE, or GLFW_REPEAT.</param>
    /// <param name="mods">Bit field describing which modifier keys were held down.</param>
    public delegate void KeyCallback(IntPtr window, int key, int scancode, int action, int mods);

    /// <summary>
    /// Sets the key callback for the specified window.
    /// </summary>
    /// <param name="window">The window whose callback to set.</param>
    /// <param name="callback">The key callback to set.</param>
    /// <returns>The previously set callback, or null if no callback was set.</returns>
    public static KeyCallback? SetKeyCallback(IntPtr window, KeyCallback callback)
    {
        // Retrieve the previous callback.
        IntPtr previousCallbackPtr = glfwSetKeyCallback(window, callback);

        // Convert the pointer to a KeyCallback delegate if it's not null.
        return previousCallbackPtr == IntPtr.Zero ? null : Marshal.GetDelegateForFunctionPointer<KeyCallback>(previousCallbackPtr);
    }

    /// <summary>
    /// Sets an input mode option for the specified window.
    /// </summary>
    /// <param name="window">The window whose input mode to set.</param>
    /// <param name="mode">The input mode to set. One of GLFW_CURSOR, GLFW_STICKY_KEYS, etc.</param>
    /// <param name="value">The value to set the input mode to. For GLFW_CURSOR, this can be GLFW_CURSOR_NORMAL, GLFW_CURSOR_HIDDEN, etc.</param>
    [LibraryImport(NativeHelperName.GLFWLibraryName, EntryPoint = "glfwSetInputMode")]
    private static partial void glfwSetInputMode(IntPtr window, int mode, int value);

    /// <summary>
    /// Sets the input mode for the specified window.
    /// </summary>
    /// <param name="window">The window whose input mode to set.</param>
    /// <param name="mode">The input mode to set. One of GLFW_CURSOR, GLFW_STICKY_KEYS, etc.</param>
    /// <param name="value">The value to set the input mode to. For GLFW_CURSOR, this can be GLFW_CURSOR_NORMAL, GLFW_CURSOR_HIDDEN, etc.</param>
    public static void SetInputMode(IntPtr window, InputMode mode, CursorMode value)
    {
        // Call the native GLFW function to set the input mode.
        glfwSetInputMode(window, (int)mode, (int)value);
    }

    /// <summary>
    /// Returns the last state reported for the specified mouse button for the specified window.
    /// </summary>
    /// <param name="window">The window to query.</param>
    /// <param name="button">The mouse button to query. One of GLFW_MOUSE_BUTTON_*.</param>
    /// <returns>GLFW_PRESS or GLFW_RELEASE.</returns>
    [LibraryImport(NativeHelperName.GLFWLibraryName, EntryPoint = "glfwGetMouseButton")]
    private static partial int glfwGetMouseButton(IntPtr window, int button);

    /// <summary>
    /// Gets the state of the specified mouse button for the given window.
    /// </summary>
    /// <param name="window">The window to query.</param>
    /// <param name="button">The mouse button to query. One of GLFW_MOUSE_BUTTON_*.</param>
    /// <returns>True if the button is pressed, otherwise false.</returns>
    public static bool GetMouseButton(IntPtr window, int button)
    {
        // Call the native GLFW function and check if the button is pressed.
        return glfwGetMouseButton(window, button) == 1; // GLFW_PRESS is defined as 1.
    }

    /// <summary>
    /// Sets the mouse button callback for the specified window, which will be called when a mouse button is pressed or released.
    /// </summary>
    /// <param name="window">The window whose callback to set.</param>
    /// <param name="callback">The new mouse button callback, or null to remove the currently set callback.</param>
    /// <returns>The previously set callback, or null if no callback was set.</returns>
    [LibraryImport(NativeHelperName.GLFWLibraryName, EntryPoint = "glfwSetMouseButtonCallback")]
    private static partial IntPtr glfwSetMouseButtonCallback(IntPtr window, MouseButtonCallback callback);

    /// <summary>
    /// Delegate for the mouse button callback function.
    /// </summary>
    /// <param name="window">The window that received the event.</param>
    /// <param name="button">The mouse button that was pressed or released.</param>
    /// <param name="action">GLFW_PRESS or GLFW_RELEASE.</param>
    /// <param name="mods">Bit field describing which modifier keys were held down.</param>
    public delegate void MouseButtonCallback(IntPtr window, int button, int action, int mods);

    /// <summary>
    /// Sets the mouse button callback for the specified window.
    /// </summary>
    /// <param name="window">The window whose callback to set.</param>
    /// <param name="callback">The mouse button callback to set.</param>
    /// <returns>The previously set callback, or null if no callback was set.</returns>
    public static MouseButtonCallback? SetMouseButtonCallback(IntPtr window, MouseButtonCallback callback)
    {
        // Retrieve the previous callback.
        IntPtr previousCallbackPtr = glfwSetMouseButtonCallback(window, callback);

        // Convert the pointer to a MouseButtonCallback delegate if it's not null.
        return previousCallbackPtr == IntPtr.Zero ? null : Marshal.GetDelegateForFunctionPointer<MouseButtonCallback>(previousCallbackPtr);
    }

    /// <summary>
    /// Delegate for cursor position callback.
    /// </summary>
    /// <param name="window">The window that received the event.</param>
    /// <param name="xpos">The x-coordinate of the cursor, relative to the left edge of the client area.</param>
    /// <param name="ypos">The y-coordinate of the cursor, relative to the top edge of the client area.</param>
    public delegate void CursorPositionCallback(IntPtr window, double xpos, double ypos);
    [LibraryImport(NativeHelperName.GLFWLibraryName, EntryPoint = "glfwSetCursorPosCallback")]
    private static partial IntPtr glfwSetCursorPosCallback(IntPtr window, CursorPositionCallback callback);

    /// <summary>
    /// Sets the cursor position callback for the specified window.
    /// </summary>
    /// <param name="window">The window whose callback to set.</param>
    /// <param name="callback">The cursor position callback to set.</param>
    /// <returns>The previously set callback, or null if no callback was set.</returns>
    public static CursorPositionCallback? SetCursorPositionCallback(IntPtr window, CursorPositionCallback callback)
    {
        IntPtr previousCallbackPtr = glfwSetCursorPosCallback(window, callback);
        return previousCallbackPtr == IntPtr.Zero ? null : Marshal.GetDelegateForFunctionPointer<CursorPositionCallback>(previousCallbackPtr);
    }
}
