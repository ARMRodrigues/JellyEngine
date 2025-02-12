using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glBlendFunc_d(uint sfactor, uint dfactor);
        private static glBlendFunc_d _glBlendFunc;

        /// <summary>
        /// Specifies pixel arithmetic for blending.
        /// </summary>
        /// <param name="sfactor">Specifies how the source blending factor is computed.</param>
        /// <param name="dfactor">Specifies how the destination blending factor is computed.</param>
        public static void BlendFunc(BlendingFactorSrc sfactor, BlendingFactorDest dfactor)
        {
            _glBlendFunc((uint)sfactor, (uint)dfactor);
        }

    }
}