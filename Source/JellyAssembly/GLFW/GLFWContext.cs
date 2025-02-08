using System.Runtime.InteropServices;

namespace JellyAssembly.GLFW
{
    public partial class GLFW
    {

        //  void glfwMakeContextCurrent(GLFWwindow* window)
        [LibraryImport(NativeHelperName.GLFWLibraryName, EntryPoint = "glfwMakeContextCurrent")]
        private static partial void glfwMakeContextCurrent(IntPtr window);
        /// <summary>
        /// Makes the context of the specified window current for the calling thread.
        /// </summary>
        /// <param name="window">The window whose context to make current, or NULL to detach the current context.</param>
        public static void MakeContextCurrent(IntPtr window)
        {
            glfwMakeContextCurrent(window);
        }

        //GLFWwindow* glfwGetCurrentContext(void)
        // 	Returns the window whose context is current on the calling thread.


        //  void glfwSwapInterval(int interval)
        [LibraryImport(NativeHelperName.GLFWLibraryName, EntryPoint = "glfwSwapInterval")]        
        private static partial void glfwSwapInterval(int interval);
        /// <summary>
        /// Sets the swap interval for the current context.
        /// </summary>
        /// <param name="interval">The minimum number of screen updates to wait for until the buffers are swapped by glfwSwapBuffers</param>
        public static void SwapInterval(int interval)
        {
            glfwSwapInterval(interval);
        }

        //int glfwExtensionSupported(const char* extension)
        // 	Returns whether the specified extension is available.

        //  GLFWglproc glfwGetProcAddress(const char* procname)
        [LibraryImport(NativeHelperName.GLFWLibraryName, EntryPoint = "glfwGetProcAddress", StringMarshalling = StringMarshalling.Utf8)]
        private static partial IntPtr glfwGetProcAddress(string procName);
        /// <summary>
        /// Returns the address of the specified function for the current context.
        /// </summary>
        /// <param name="procName">The ASCII encoded name of the function.</param>
        /// <returns>The address of the function, or NULL if an error occurred.</returns>
        public static IntPtr GetProcAddress(string procName)
        {
            return glfwGetProcAddress(procName);
        }
    }
}
