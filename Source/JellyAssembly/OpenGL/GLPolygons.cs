using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glCullFace_d(uint mode);
        private static glCullFace_d _glCullFace;

        /// <summary>
        /// Specifies whether front- or back-facing facets are culled.
        /// </summary>
        /// <param name="mode">Specifies which faces to cull.</param>
        public static void CullFace(CullFaceMode mode)
        {
            _glCullFace((uint)mode);
        }

    }
}