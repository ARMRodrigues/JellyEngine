using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr glGetString_d(uint name);
        private static glGetString_d _glGetString;
        private static string glGetString(uint name)
        {
            nint resultPtr = _glGetString(name);

            if (resultPtr == IntPtr.Zero)
                throw new InvalidOperationException("glGetString returned a null pointer.");

            return Marshal.PtrToStringAnsi(resultPtr) ?? throw new InvalidOperationException("Failed to marshal string.");
        }
        /// <summary>
        /// Return a string describing the current GL connection.
        /// </summary>
        /// <param name="name">Specifies a symbolic constant, one of GL_VENDOR, GL_RENDERER, GL_VERSION, or GL_SHADING_LANGUAGE_VERSION.</param>
        /// <returns></returns>
        public static string GetString(StringName name)
        {
            return glGetString((uint)name);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr glGetStringi_d(uint name, uint index);
        private static glGetStringi_d _glGetStringi;
        /// <summary>
        /// Return a string describing the current GL connection, this specifies the index of the string to return.
        /// </summary>
        /// <param name="name">Specifies a symbolic constant, one of GL_VENDOR, GL_RENDERER, GL_VERSION, or GL_SHADING_LANGUAGE_VERSION.</param>
        /// <param name="index">Specifies a symbolic constant, one of GL_VENDOR, GL_RENDERER, GL_VERSION, or GL_SHADING_LANGUAGE_VERSION.</param>
        /// <returns>Returns a pointer to a static string indexed by index.</returns>
        public static string GetStringIndex(StringName name, uint index)
        {
            var resultPtr = _glGetStringi((uint)name, index);

            if (resultPtr == IntPtr.Zero)
                throw new InvalidOperationException("glGetStringi returned a null pointer.");

            return Marshal.PtrToStringAnsi(resultPtr) ?? throw new InvalidOperationException("Failed to marshal string.");
        } 
    }
}
