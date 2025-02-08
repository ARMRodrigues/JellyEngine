using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glGenBuffers_d(int n, uint[] buffers);
        private static glGenBuffers_d _glGenBuffers;
        /// <summary>
        /// Generate buffer object names.
        /// </summary>
        /// <param name="n">Specifies the number of buffer object names to be generated.</param>
        /// <returns>Returns an array index in which the generated buffer object names are stored</returns>
        public static uint GenBuffers()
        {
            var buffer = new uint[1];
            _glGenBuffers(1, buffer);
            return buffer[0];
        }
        public static uint[] GenBuffers(int n)
        {
            // TODO :Not tested 
            var buffers = new uint[1];
            _glGenBuffers(n, buffers);
            return buffers;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glDeleteBuffers_d(int n, ref uint buffers);
        private static glDeleteBuffers_d _glDeleteBuffers;
        /// <summary>
        /// delete named buffer objects
        /// </summary>
        /// <param name="n">Specifies the number of buffer objects to be deleted.</param>
        /// <param name="buffers">Specifies an array index of buffer objects to be deleted.</param>
        public static void DeleteBuffers(int n, uint buffers)
        {
            _glDeleteBuffers(n, ref buffers);
        }
    }
}
