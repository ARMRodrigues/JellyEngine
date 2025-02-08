using System.Runtime.InteropServices;
using System.Threading;
using JellyAssembly.GLFW;

namespace JellyAssembly.GLFW
{
    public partial class GLFW
    {
        //void glfwDefaultWindowHints

        //void glfwWindowHint
        [LibraryImport(NativeHelperName.GLFWLibraryName, EntryPoint = "glfwWindowHint")]
        private static partial void glfwWindowHint(int hint, int value);
        /// <summary>Sets the specified window hint to the desired value.</summary>
        /// <param name="windowHint">The window hint to set.</param>
        /// <param name="value">The new value of the window hint, use GLFWBool enum.</param>
        public static void WindowHint(WindowHint windowHint, GLFWBool value)
        {
            glfwWindowHint((int)windowHint, (int)value);
        }
        /// <summary>Sets the specified window hint to the desired value.</summary>
        /// <param name="windowHint">The window hint to set.</param>
        /// <param name="value">The new value of the window hint, use OpenGLProfile enum.</param>
        public static void WindowHint(WindowHint windowHint, OpenGLProfile value)
        {
            glfwWindowHint((int)windowHint, (int)value);
        }
        /// <summary>Sets the specified window hint to the desired value.</summary>
        /// <param name="windowHint">The window hint to set.</param>
        /// <param name="value">The new value of the window hint.</param>
        public static void WindowHint(WindowHint windowHint, int value)
        {
            glfwWindowHint((int)windowHint, value);
        }

        //void glfwWindowHintString

        //  GLFWwindow glfwCreateWindow
        [LibraryImport(NativeHelperName.GLFWLibraryName, EntryPoint = "glfwCreateWindow", StringMarshalling = StringMarshalling.Utf8)]
        public static partial IntPtr glfwCreateWindow(int width, int height, string title, IntPtr monitor, IntPtr share);
        /// <summary>
        /// Creates a window and its associated context.
        /// </summary>
        /// <param name="width">The desired width, in screen coordinates, of the window. This must be greater than zero.</param>
        /// <param name="height">The desired height, in screen coordinates, of the window. This must be greater than zero.</param>
        /// <param name="title">The initial, UTF-8 encoded window title.</param>
        /// <param name="monitor">The monitor to use for full screen mode, or NULL for windowed mode.</param>
        /// <param name="share">The window whose context to share resources with, or NULL to not share resources.</param>
        /// <returns>The handle of the created window, or NULL if an error occurred.</returns>
        public static IntPtr CreateWindow(int width, int height, string title, IntPtr monitor, IntPtr share)
        {
            return glfwCreateWindow(width, height, title, monitor, share);
        }

        //  void glfwDestroyWindow
        [LibraryImport(NativeHelperName.GLFWLibraryName, EntryPoint = "glfwDestroyWindow")]
        private static partial void glfwDestroyWindow(IntPtr window);
        /// <summary>
        /// This function destroys the specified window and its context. On calling this function, no further callbacks will be called for that window.
        /// If the context of the specified window is current on the main thread, it is detached before being destroyed.
        /// </summary>
        /// <param name="window">The window to destroy.</param>
        public static void DestroyWindow(IntPtr window)
        {
            glfwDestroyWindow(window);
        }

        //  int glfwWindowShouldClose
        [LibraryImport(NativeHelperName.GLFWLibraryName, EntryPoint = "glfwWindowShouldClose")]
        private static partial int glfwWindowShouldClose(IntPtr window);
        /// <summary>
        /// This function returns the value of the close flag of the specified window.
        /// </summary>
        /// <param name="window">The window to query.</param>
        /// <returns></returns>
        public static bool WindowShouldClose(IntPtr window)
        {
            return glfwWindowShouldClose(window) == 1 ? true : false;
        }

        //void glfwSetWindowShouldClose
        //void glfwSetWindowTitle
        //void glfwSetWindowIcon
        //void glfwGetWindowPos

        //void glfwSetWindowPos
        /// <summary>
        /// Sets the position of the client area of the specified window.
        /// </summary>
        /// <param name="window">The window whose position to set.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the client area.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the client area.</param>
        [LibraryImport(NativeHelperName.GLFWLibraryName, EntryPoint = "glfwSetWindowPos")]
        private static partial void glfwSetWindowPos(IntPtr window, int x, int y);

        /// <summary>
        /// Sets the position of the client area of the specified window.
        /// </summary>
        /// <param name="window">The window whose position to set.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the client area.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the client area.</param>
        public static void SetWindowPos(IntPtr window, int x, int y)
        {
            // Call the native GLFW function to set the window position.
            glfwSetWindowPos(window, x, y);
        }

        //void glfwGetWindowSize
        /// <summary>
        /// Retrieves the size of the client area of the specified window.
        /// </summary>
        /// <param name="window">The window whose size to retrieve.</param>
        /// <param name="width">The width of the client area.</param>
        /// <param name="height">The height of the client area.</param>
        [LibraryImport(NativeHelperName.GLFWLibraryName, EntryPoint = "glfwGetWindowSize")]
        private static partial void glfwGetWindowSize(IntPtr window, out int width, out int height);

        /// <summary>
        /// Gets the size of the client area of the specified window.
        /// </summary>
        /// <param name="window">The window whose size to retrieve.</param>
        /// <returns>A tuple containing the width and height of the client area.</returns>
        public static (int Width, int Height) GetWindowSize(IntPtr window)
        {
            // Call the native GLFW function to get the window size.
            glfwGetWindowSize(window, out int width, out int height);

            // Return the width and height as a tuple.
            return (width, height);
        }

        //void glfwSetWindowSizeLimits
        //void glfwSetWindowAspectRatio
        //void glfwSetWindowSize
        //void glfwGetFramebufferSize
        //void glfwGetWindowFrameSize
        //void glfwGetWindowContentScale
        //float glfwGetWindowOpacity
        //void glfwSetWindowOpacity
        //void glfwIconifyWindow
        //void glfwRestoreWindow
        //void glfwMaximizeWindow

        //void glfwShowWindow
        /// <summary>
        /// Shows the specified window.
        /// </summary>
        /// <param name="window">The window to show.</param>
        [LibraryImport(NativeHelperName.GLFWLibraryName, EntryPoint = "glfwShowWindow")]
        private static partial void glfwShowWindow(IntPtr window);

        /// <summary>
        /// Shows the specified window.
        /// </summary>
        /// <param name="window">The window to show.</param>
        public static void ShowWindow(IntPtr window)
        {
            // Call the native GLFW function to show the window.
            glfwShowWindow(window);
        }

        //void glfwHideWindow
        /// <summary>
        /// Hides the specified window.
        /// </summary>
        /// <param name="window">The window to hide.</param>
        [LibraryImport(NativeHelperName.GLFWLibraryName, EntryPoint = "glfwHideWindow")]
        private static partial void glfwHideWindow(IntPtr window);

        /// <summary>
        /// Hides the specified window.
        /// </summary>
        /// <param name="window">The window to hide.</param>
        public static void HideWindow(IntPtr window)
        {
            // Call the native GLFW function to hide the window.
            glfwHideWindow(window);
        }

        //void glfwFocusWindow
        //void glfwRequestWindowAttention
        //GLFWmonitor glfwGetWindowMonitor
        //void glfwSetWindowMonitor
        //int glfwGetWindowAttrib
        //void glfwSetWindowAttrib
        //void glfwSetWindowUserPointer
        //void glfwGetWindowUserPointer
        //GLFWwindowposfun glfwSetWindowPosCallback
        //GLFWwindowsizefun glfwSetWindowSizeCallback
        //GLFWwindowclosefun glfwSetWindowCloseCallback
        //GLFWwindowrefreshfun glfwSetWindowRefreshCallback
        //GLFWwindowfocusfun glfwSetWindowFocusCallback
        //GLFWwindowiconifyfun glfwSetWindowIconifyCallback
        //GLFWwindowmaximizefun glfwSetWindowMaximizeCallback

        //GLFWframebuffersizefun glfwSetFramebufferSizeCallback
        [LibraryImport(NativeHelperName.GLFWLibraryName, EntryPoint = "glfwSetFramebufferSizeCallback")]
        private static partial void glfwSetFramebufferSizeCallback(IntPtr window, IntPtr callback);

        /// <summary>
        /// Public method to bind the framebuffer size callback.
        /// </summary>
        /// <param name="window">Handle to the window.</param>
        /// <param name="callback">Action to handle framebuffer size changes.</param>
        public static void SetFramebufferSizeCallback(IntPtr window, Action<int, int> callback)
        {
            GLFWCallbacks.OnFramebufferSizeChanged += callback;
            glfwSetFramebufferSizeCallback(window, GLFWCallbacks.FramebufferSizeCallbackPointer); // Set up the native binding.
        }

        //GLFWwindowcontentscalefun glfwSetWindowContentScaleCallback

        //void glfwPollEvents
        [LibraryImport(NativeHelperName.GLFWLibraryName, EntryPoint = "glfwPollEvents")]
        private static partial void glfwPollEvents();
        /// <summary>
        /// Processes all pending events.
        /// </summary>
        public static void PollEvents()
        {
            glfwPollEvents();
        }

        //void glfwWaitEvents
        //void glfwWaitEventsTimeout
        //void glfwPostEmptyEvent

        //void glfwSwapBuffers
        [LibraryImport(NativeHelperName.GLFWLibraryName, EntryPoint = "glfwSwapBuffers")]
        private static partial void glfwSwapBuffers(IntPtr window);
        /// <summary>
        /// Swaps the front and back buffers of the specified window.
        /// </summary>
        /// <param name="window">The window whose buffers to swap.</param>
        public static void SwapBuffers(IntPtr window)
        {
            glfwSwapBuffers(window);
        }

    }
}
