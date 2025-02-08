using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glFramebufferRenderbuffer_d(
            uint target, 
            uint attachment, 
            uint renderbuffertarget, 
            uint renderbuffer);
        private static glFramebufferRenderbuffer_d _glFramebufferRenderbuffer;

        /// <summary>
        /// Attach a renderbuffer as a logical buffer of a framebuffer object.
        /// </summary>
        /// <param name="target">Specifies the target framebuffer (e.g., GL_FRAMEBUFFER).</param>
        /// <param name="attachment">Specifies the attachment point of the framebuffer (e.g., GL_COLOR_ATTACHMENT0).</param>
        /// <param name="renderbufferTarget">Specifies the renderbuffer target (e.g., GL_RENDERBUFFER).</param>
        /// <param name="renderbuffer">Specifies the name of the renderbuffer object to attach.</param>
        public static void FramebufferRenderbuffer(
            FramebufferTarget target, 
            GLFramebufferAttachment attachment, 
            RenderbufferTarget renderbufferTarget, 
            uint renderbuffer)
        {
            _glFramebufferRenderbuffer((uint)target, (uint)attachment, (uint)renderbufferTarget, renderbuffer);
        }

    }
}