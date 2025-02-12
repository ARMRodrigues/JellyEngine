using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glDepthMask_d(bool flag);
        private static glDepthMask_d _glDepthMask;

        /// <summary>
        /// Enables or disables writing into the depth buffer.
        /// </summary>
        /// <param name="flag">Specifies whether the depth buffer is enabled for writing.</param>
        public static void DepthMask(bool flag)
        {
            _glDepthMask(flag);
        }

    }
}