using System.Runtime.InteropServices;
using System.Text;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public unsafe partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint glCreateShader_d(uint shaderType);
        private static glCreateShader_d _glCreateShader;
        /// <summary>
        /// Creates a shader object.
        /// </summary>
        /// <param name="shaderType">Specifies the type of shader to be created. Must be one of GL_COMPUTE_SHADER, GL_VERTEX_SHADER, GL_TESS_CONTROL_SHADER, GL_TESS_EVALUATION_SHADER, GL_GEOMETRY_SHADER, or GL_FRAGMENT_SHADER</param>
        /// <returns>CreateShader creates an empty shader object and returns a non-zero value by which it can be referenced.</returns>
        public static uint CreateShader(ShaderType shaderType)
        {
            return _glCreateShader((uint)shaderType);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glShaderSource_d(uint shader, int count, IntPtr[] strings, IntPtr lengths);
        private static glShaderSource_d _glShaderSource;
        /// <summary>
        /// Replaces the source code in a shader object
        /// </summary>
        /// <param name="shader">Specifies the handle of the shader object whose source code is to be replaced.</param>
        /// <param name="count">Specifies the number of elements in the string and length arrays.</param>
        /// <param name="strings">Specifies an array of pointers to strings containing the source code to be loaded into the shader.</param>
        /// <param name="lengths">pecifies an array of string lengths.</param>
        public static void ShaderSource(uint shader, int count, string strings, IntPtr lengths)
        {
            var shaderSourcePtr = Marshal.StringToHGlobalAnsi(strings);
            _glShaderSource(shader, count, [shaderSourcePtr], lengths);
            Marshal.FreeHGlobal(shaderSourcePtr);
        }

        //public unsafe static void glShaderSource(uint shader, int count, byte** str, int* length) => _glShaderSource(shader, count, str, length);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void glCompileShader(uint shader);

        private static glCompileShader _glCompileShader;
        /// <summary>
        /// Compiles the source code strings that have been stored in the shader object specified by shader.
        /// </summary>
        /// <param name="shader">Specifies the shader object to be compiled.</param>
        public static void CompileShader(uint shader)
        {
            _glCompileShader(shader);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glDeleteShader_d(uint shader);
        private static glDeleteShader_d _glDeleteShader;
        /// <summary>
        /// Frees the memory and invalidates the name associated with the shader object specified by shader. 
        /// </summary>
        /// <param name="shader">Specifies the shader object to be deleted.</param>
        public static void DeleteShader(uint shader)
        {
            _glDeleteShader(shader);
        }
    }
}
