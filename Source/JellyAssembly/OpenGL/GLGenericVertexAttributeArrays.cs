using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glVertexAttribPointer_d(uint index, int size, uint type, bool normalized, int stride, IntPtr pointer);
        private static glVertexAttribPointer_d _glVertexAttribPointer;
        /// <summary>
        /// Define an array of generic vertex attribute data
        /// </summary>
        /// <param name="index">Specifies the index of the generic vertex attribute to be modified.</param>
        /// <param name="size">Specifies the number of components per generic vertex attribute. Must be 1, 2, 3, 4. Additionally, the symbolic constant GL_BGRA is accepted by glVertexAttribPointer. The initial value is 4.</param>
        /// <param name="type">Specifies the data type of each component in the array.</param>
        /// <param name="normalized">Specifies whether fixed-point data values should be normalized (GL_TRUE) or converted directly as fixed-point values (GL_FALSE) when they are accessed.</param>
        /// <param name="stride">Specifies the byte offset between consecutive generic vertex attributes. If stride is 0, the generic vertex attributes are understood to be tightly packed in the array. The initial value is 0.</param>
        /// <param name="pointer">Specifies a offset of the first component of the first generic vertex attribute in the array in the data store of the buffer currently bound to the GL_ARRAY_BUFFER target. The initial value is 0.</param>
        public static void VertexAttribPointer(uint index, int size, VertexAttribPointerType type, bool normalized, int stride, IntPtr pointer)
        {
            _glVertexAttribPointer(index, size, (uint)type, normalized, stride, pointer);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glEnableVertexAttribArray_d(uint index);
        private static glEnableVertexAttribArray_d _glEnableVertexAttribArray;
        /// <summary>
        /// Enable a generic vertex attribute array.
        /// </summary>
        /// <param name="index">Specifies the index of the generic vertex attribute to be disabled.</param>
        public static void EnableVertexAttribArray(uint index)
        {
            _glEnableVertexAttribArray(index);
        }
    }
}
