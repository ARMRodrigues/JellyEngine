using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
#pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glBlendEquationSeparate_d(uint modeRGB, uint modeAlpha);
        private static glBlendEquationSeparate_d _glBlendEquationSeparate;

        /// <summary>
        /// Sets the blend equation separately for RGB and alpha components.
        /// </summary>
        /// <param name="modeRGB">Specifies the blend equation for the RGB components.</param>
        /// <param name="modeAlpha">Specifies the blend equation for the alpha component.</param>
        public static void BlendEquationSeparate(BlendEquationMode modeRGB, BlendEquationMode modeAlpha)
        {
            _glBlendEquationSeparate((uint)modeRGB, (uint)modeAlpha);
        }

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

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glBlendFuncSeparate_d(uint sfactorRGB, uint dfactorRGB, uint sfactorAlpha, uint dfactorAlpha);
        private static glBlendFuncSeparate_d _glBlendFuncSeparate;

        /// <summary>
        /// Specifies pixel arithmetic for RGB and alpha components separately.
        /// </summary>
        /// <param name="sfactorRGB">Specifies how the red, green, and blue blending factors are computed for the RGB blend equation.</param>
        /// <param name="dfactorRGB">Specifies how the red, green, and blue destination blending factors are computed for the RGB blend equation.</param>
        /// <param name="sfactorAlpha">Specifies how the alpha source blending factor is computed for the alpha blend equation.</param>
        /// <param name="dfactorAlpha">Specifies how the alpha destination blending factor is computed for the alpha blend equation.</param>
        public static void BlendFuncSeparate(BlendingFactorSrc sfactorRGB, BlendingFactorDest dfactorRGB, BlendingFactorSrc sfactorAlpha, BlendingFactorDest dfactorAlpha)
        {
            _glBlendFuncSeparate((uint)sfactorRGB, (uint)dfactorRGB, (uint)sfactorAlpha, (uint)dfactorAlpha);
        }

    }
}