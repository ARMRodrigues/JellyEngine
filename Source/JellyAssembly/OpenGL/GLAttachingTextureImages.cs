using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glFramebufferTexture2D_d(
            uint target, 
            uint attachment, 
            uint textarget, 
            uint texture, 
            int level);
        private static glFramebufferTexture2D_d _glFramebufferTexture2D;

        /// <summary>
        /// Attach a texture to a framebuffer object.
        /// </summary>
        /// <param name="target">Specifies the target of the framebuffer (e.g., GL_FRAMEBUFFER).</param>
        /// <param name="attachment">Specifies the attachment point of the framebuffer (e.g., GL_COLOR_ATTACHMENT0).</param>
        /// <param name="textarget">Specifies the texture target (e.g., GL_TEXTURE_2D).</param>
        /// <param name="texture">Specifies the name of the texture to attach.</param>
        /// <param name="level">Specifies the mipmap level of the texture to attach.</param>
        public static void FramebufferTexture2D(
            FramebufferTarget target, 
            GLFramebufferAttachment attachment, 
            TextureTarget textarget, 
            uint texture, 
            int level)
        {
            _glFramebufferTexture2D((uint)target, (uint)attachment, (uint)textarget, texture, level);
        }

    }
}