using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glGetShaderiv_d(uint shader, uint pname, out int param);
        private static glGetShaderiv_d _glGetShaderiv;
        /// <summary>
        ///  Return a parameter from a shader object
        /// </summary>
        /// <param name="shader">Specifies the shader object to be queried.</param>
        /// <param name="pname">Specifies the object parameter. Accepted symbolic names are GL_SHADER_TYPE, GL_DELETE_STATUS, GL_COMPILE_STATUS, GL_INFO_LOG_LENGTH, GL_SHADER_SOURCE_LENGTH.</param>
        /// <returns>Returns the requested object parameter.</returns>
        public static int GetShaderiv(uint shader, ShaderParameter pname)
        {
            _glGetShaderiv(shader, (uint)pname, out int param);
            return param;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glGetProgramiv_d(uint program, uint pname, out int param);
        private static glGetProgramiv_d _glGetProgramiv;
        /// <summary>
        ///  Return a parameter from a program object.
        /// </summary>
        /// <param name="program">Specifies the program object to be queried.</param>
        /// <param name="pname">Specifies the object parameter. Accepted symbolic names are GL_DELETE_STATUS, GL_LINK_STATUS, GL_VALIDATE_STATUS, GL_INFO_LOG_LENGTH, GL_ATTACHED_SHADERS, GL_ACTIVE_ATTRIBUTES, GL_ACTIVE_ATTRIBUTE_MAX_LENGTH, GL_ACTIVE_UNIFORMS, GL_ACTIVE_UNIFORM_MAX_LENGTH.</param>
        /// <returns>Returns the value of a parameter for a specific program object.</returns>
        public static int GetProgramiv(uint program, GetProgramParameterName pname)
        {
            int param;
            _glGetProgramiv(program, (uint)pname, out param);
            return param;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glGetShaderInfoLog_d(uint shader, int maxLength, out int length, IntPtr infoLog);
        private static glGetShaderInfoLog_d _glGetShaderInfoLog;
        /// <summary>
        /// Retrieves the information log for a shader object.
        /// </summary>
        /// <param name="shader">Specifies the shader object whose information log is to be queried.</param>
        /// <param name="maxLength">Specifies the size of the character buffer for storing the returned information log.</param>
        /// <returns>Returns the information log for the specified shader object.</returns>
        public static string GetShaderInfoLog(uint shader, int maxLength)
        {
            var infoLogPtr = Marshal.AllocHGlobal(maxLength);
            try
            {
                _glGetShaderInfoLog(shader, maxLength, out _, infoLogPtr);
                return Marshal.PtrToStringAnsi(infoLogPtr) ?? string.Empty;
            }
            finally
            {
                Marshal.FreeHGlobal(infoLogPtr);
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glGetProgramInfoLog_d(uint program, int maxLength, out int length, IntPtr infoLog);
        private static glGetProgramInfoLog_d _glGetProgramInfoLog;
        /// <summary>
        /// Returns the information log for a program object.
        /// </summary>
        /// <param name="program">Specifies the program object whose information log is to be queried.</param>
        /// <param name="maxLength">Specifies the size of the character buffer for storing the returned information log.</param>
        /// <returns>Returns the information log for the specified program object.</returns>
        public static string GetProgramInfoLog(uint program, int maxLength)
        {
            var infoLogPtr = Marshal.AllocHGlobal(maxLength);
            try
            {
                _glGetProgramInfoLog(program, maxLength, out int length, infoLogPtr);
                return Marshal.PtrToStringAnsi(infoLogPtr, length) ?? string.Empty;
            }
            finally
            {
                Marshal.FreeHGlobal(infoLogPtr);
            }
        }
    }
}
