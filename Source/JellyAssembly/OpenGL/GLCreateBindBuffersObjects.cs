using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glBindBuffer_d(uint target, uint buffer);
        private static glBindBuffer_d _glBindBuffer;
        /// <summary>
        /// Bind a named buffer object.
        /// </summary>
        /// <param name="target">Specifies the target to which the buffer object is bound, which must be one of the buffer binding targets in the following BufferTarget enum.</param>
        /// <param name="buffer">Specifies the name of a buffer object</param>
        public static void BindBuffer(BufferTarget target, uint buffer)
        {
            _glBindBuffer((uint)target, buffer);
        }
    }
}
