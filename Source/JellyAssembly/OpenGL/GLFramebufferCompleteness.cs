using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint glCheckFramebufferStatus_d(uint target);
        private static glCheckFramebufferStatus_d _glCheckFramebufferStatus;

        /// <summary>
        /// Check the status of a framebuffer object.
        /// </summary>
        /// <param name="target">Specifies the target framebuffer (e.g., GL_FRAMEBUFFER).</param>
        /// <returns>The status of the framebuffer (e.g., GL_FRAMEBUFFER_COMPLETE).</returns>
        public static FramebufferErrorCode CheckFramebufferStatus(FramebufferTarget target)
        {
            return (FramebufferErrorCode)_glCheckFramebufferStatus((uint)target);
        }

    }
}