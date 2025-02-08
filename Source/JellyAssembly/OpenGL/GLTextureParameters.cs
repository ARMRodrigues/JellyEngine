using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glTexParameterf_d(uint target, uint pname, float param);
        private static glTexParameterf_d _glTexParameterf;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glTexParameteri_d(uint target, uint pname, int param);
        private static glTexParameteri_d _glTexParameteri;

        /// <summary>
        /// Set a floating-point texture parameter.
        /// </summary>
        /// <param name="target">Specifies the texture target (e.g., GL_TEXTURE_2D).</param>
        /// <param name="pname">Specifies the symbolic name of a single-valued texture parameter (e.g., GL_TEXTURE_MIN_FILTER).</param>
        /// <param name="param">Specifies the value of the parameter.</param>
        public static void zTexParameter(TextureTarget target, TextureParameterName pname, float param)
        {
            _glTexParameterf((uint)target, (uint)pname, param);
        }

        /// <summary>
        /// Set an integer texture parameter.
        /// </summary>
        /// <param name="target">Specifies the texture target (e.g., GL_TEXTURE_2D).</param>
        /// <param name="pname">Specifies the symbolic name of a single-valued texture parameter (e.g., GL_TEXTURE_MIN_FILTER).</param>
        /// <param name="param">Specifies the value of the parameter.</param>
        public static void TexParameter(TextureTarget target, TextureParameterName pname, int param)
        {
            _glTexParameteri((uint)target, (uint)pname, param);
        }
    }
}