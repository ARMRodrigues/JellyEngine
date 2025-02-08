using System.Runtime.InteropServices;

namespace JellyAssembly.GLFW
{
    public partial class GLFW
    {
        //  int glfwInit(void)
        [LibraryImport(NativeHelperName.GLFWLibraryName, EntryPoint = "glfwInit")]
        private static partial int glfwInit();
        /// <summary>
        /// Initializes the GLFW library.
        /// </summary>
        public static int Init()
        {
            return glfwInit();
        }

        //  void glfwTerminate (void)
        [LibraryImport(NativeHelperName.GLFWLibraryName, EntryPoint = "glfwTerminate")]
        private static partial void glfwTerminate();
        /// <summary>
        /// Terminates the GLFW library.
        /// </summary>
        public static void Terminate()
        {
            glfwTerminate();
        }

        //  void glfwInitHint (int hint, int value)
        //  Sets the specified init hint to the desired value.


        //void glfwGetVersion (int* major, int* minor, int* rev)
        //     Retrieves the version of the GLFW library.


        //const char* glfwGetVersionString (void)
        //     Returns a string describing the compile-time configuration.


        //int glfwGetError (const char** description)
        [LibraryImport(NativeHelperName.GLFWLibraryName, EntryPoint = "glfwGetError")]
        private static partial int glfwGetError(out IntPtr description);
        /// <summary>
        /// Returns and clears the last error for the calling thread.
        /// </summary>
        public static string GetGLFWError()
        {
            IntPtr errorDescriptionPtr = IntPtr.Zero;
            int errorCode = glfwGetError(out errorDescriptionPtr);

            if (errorCode != 0 && errorDescriptionPtr != IntPtr.Zero)
            {
                return Marshal.PtrToStringAnsi(errorDescriptionPtr) ?? "Unknown error";
            }

            return string.Empty;
        }

        //GLFWerrorfun glfwSetErrorCallback(GLFWerrorfun callback)
        //     Sets the error callback.
    }
}
