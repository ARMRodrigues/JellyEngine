using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glDepthFunc_d(uint func);
        private static glDepthFunc_d _glDepthFunc;
        /// <summary>
        /// Specifies the function used to compare each incoming pixel depth value with the depth value present in the depth buffer.
        /// </summary>
        /// <param name="func">Specifies the depth comparison function. Symbolic constants GL_NEVER, GL_LESS, GL_EQUAL, GL_LEQUAL, GL_GREATER, GL_NOTEQUAL, GL_GEQUAL, and GL_ALWAYS are accepted. The initial value is GL_LESS</param>
        public static void DepthFunc(DepthFunction func)
        {
            _glDepthFunc((uint)func);
        }
    }
}
