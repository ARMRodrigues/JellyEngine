using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glBindRenderbuffer_d(uint target, uint renderbuffer);
        private static glBindRenderbuffer_d _glBindRenderbuffer;

        /// <summary>
        /// Bind a named renderbuffer object.
        /// </summary>
        /// <param name="target">Specifies the target to which the renderbuffer is bound (e.g., GL_RENDERBUFFER).</param>
        /// <param name="renderbuffer">Specifies the name of the renderbuffer object to bind.</param>
        public static void BindRenderbuffer(RenderbufferTarget target, uint renderbuffer)
        {
            _glBindRenderbuffer((uint)target, renderbuffer);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glGenRenderbuffers_d(uint n, uint[] renderbuffers);
        private static glGenRenderbuffers_d _glGenRenderbuffers;

        /// <summary>
        /// Generate a single renderbuffer object name.
        /// </summary>
        /// <returns>The ID of the generated renderbuffer object.</returns>
        public static uint GenRenderbuffer()
        {
            uint[] renderbuffers = new uint[1];
            _glGenRenderbuffers(1, renderbuffers);
            return renderbuffers[0];
        }

        /// <summary>
        /// Generate multiple renderbuffer object names.
        /// </summary>
        /// <param name="n">Specifies the number of renderbuffer object names to generate.</param>
        /// <returns>An array containing the generated renderbuffer object names.</returns>
        public static uint[] GenRenderbuffers(uint n)
        {
            uint[] renderbuffers = new uint[n];
            _glGenRenderbuffers(n, renderbuffers);
            return renderbuffers;
        }
    
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glRenderbufferStorage_d(uint target, uint internalformat, int width, int height);
        private static glRenderbufferStorage_d _glRenderbufferStorage;

        /// <summary>
        /// Establish data storage, format, and dimensions of a renderbuffer object's image.
        /// </summary>
        /// <param name="target">Specifies the target to which the renderbuffer is bound (e.g., GL_RENDERBUFFER).</param>
        /// <param name="internalformat">Specifies the internal format to be used for the renderbuffer object's image (e.g., GL_DEPTH_COMPONENT24).</param>
        /// <param name="width">Specifies the width of the renderbuffer, in pixels.</param>
        /// <param name="height">Specifies the height of the renderbuffer, in pixels.</param>
        public static void RenderbufferStorage(RenderbufferTarget target, PixelInternalFormat internalformat, int width, int height)
        {
            _glRenderbufferStorage((uint)target, (uint)internalformat, width, height);
        }

    
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glDeleteRenderbuffers_d(uint n, uint[] renderbuffers);
        private static glDeleteRenderbuffers_d _glDeleteRenderbuffers;

        /// <summary>
        /// Delete renderbuffer objects.
        /// </summary>
        /// <param name="renderbuffer">The ID of the renderbuffer to delete.</param>
        public static void DeleteRenderbuffer(uint renderbuffer)
        {
            _glDeleteRenderbuffers(1, new uint[] { renderbuffer });
        }

        /// <summary>
        /// Delete multiple renderbuffer objects.
        /// </summary>
        /// <param name="renderbuffers">An array containing the IDs of renderbuffers to delete.</param>
        public static void DeleteRenderbuffers(uint[] renderbuffers)
        {
            _glDeleteRenderbuffers((uint)renderbuffers.Length, renderbuffers);
        }

    }
}