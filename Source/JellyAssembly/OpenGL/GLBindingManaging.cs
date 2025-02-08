using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glBindFramebuffer_d(uint target, uint framebuffer);
        private static glBindFramebuffer_d _glBindFramebuffer;
        /// <summary>
        /// Bind a named framebuffer to a framebuffer target.
        /// </summary>
        /// <param name="target">Specifies the framebuffer target (e.g., GL_FRAMEBUFFER).</param>
        /// <param name="framebuffer">Specifies the name of the framebuffer object to bind.</param>
        public static void BindFramebuffer(FramebufferTarget target, uint framebuffer)
        {
            _glBindFramebuffer((uint)target, framebuffer);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glGenFramebuffers_d(int n, uint[] framebuffers);
        private static glGenFramebuffers_d _glGenFramebuffers;
        /// <summary>
        /// Generate framebuffer object names.
        /// </summary>
        /// <param name="n">Specifies the number of framebuffer object names to generate.</param>
        /// <returns>Outputs the generated framebuffer object names.</returns>
        public static uint GenFramebuffers()
        {
            var arrays = new uint[1];
            _glGenFramebuffers(1, arrays);
            return arrays[0];
        }

        /// <summary>
        /// Generate framebuffer object names.
        /// </summary>
        /// <param name="n">Specifies the number of framebuffer object names to generate.</param>
        /// <returns>Outputs the generated framebuffer object names.</returns>
        public static uint[] GenFramebuffers(int n)
        {
            var arrays = new uint[n];
            _glGenFramebuffers(n, arrays);
            return arrays;
        }
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glDeleteFramebuffers_d(uint n, uint[] framebuffers);
        private static glDeleteFramebuffers_d _glDeleteFramebuffers;

        /// <summary>
        /// Delete framebuffer objects.
        /// </summary>
        /// <param name="framebuffer">The framebuffer ID to delete.</param>
        public static void DeleteFramebuffer(uint framebuffer)
        {
            _glDeleteFramebuffers(1, new uint[] { framebuffer });
        }

        /// <summary>
        /// Delete multiple framebuffer objects.
        /// </summary>
        /// <param name="framebuffers">An array containing framebuffer IDs to delete.</param>
        public static void DeleteFramebuffers(uint[] framebuffers)
        {
            _glDeleteFramebuffers((uint)framebuffers.Length, framebuffers);
        }

    }
}
