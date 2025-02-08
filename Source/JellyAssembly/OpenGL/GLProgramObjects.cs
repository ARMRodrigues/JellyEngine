using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint glCreateProgram_d();
        private static glCreateProgram_d _glCreateProgram;
        /// <summary>
        /// creates an empty program object and returns a non-zero value by which it can be referenced.
        /// </summary>
        /// <returns>Returns a non-zero value by which it can be referenced.</returns>
        public static uint CreateProgram()
        {
            return _glCreateProgram();
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glAttachShader_d(uint program, uint shader);
        private static glAttachShader_d _glAttachShader;
        /// <summary>
        /// Attaches a shader object to a program object
        /// </summary>
        /// <param name="program">Specifies the program object to which a shader object will be attached.</param>
        /// <param name="shader">Specifies the shader object that is to be attached.</param>
        public static void AttachShader(uint program, uint shader)
        {
            _glAttachShader(program, shader);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glLinkProgram_d(uint program);
        private static glLinkProgram_d _glLinkProgram;
        /// <summary>
        /// Links a program object.
        /// </summary>
        /// <param name="program">Specifies the handle of the program object to be linked.</param>
        public static void LinkProgram(uint program)
        {
            _glLinkProgram(program);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glUseProgram_d(uint program);
        private static glUseProgram_d _glUseProgram;
        /// <summary>
        /// UseProgram installs the program object specified by program as part of current rendering state. 
        /// </summary>
        /// <param name="program">Specifies the handle of the program object whose executables are to be used as part of current rendering state.</param>
        public static void UseProgram(uint program)
        {
            _glUseProgram(program);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glDeleteProgram_d(uint program);
        private static glDeleteProgram_d _glDeleteProgram;
        /// <summary>
        /// Frees the memory and invalidates the name associated with the program object specified by program.
        /// </summary>
        /// <param name="program">Specifies the program object to be deleted.</param>
        public static void DeleteProgram(uint program)
        {
            _glDeleteProgram(program);
        }
    }
}
