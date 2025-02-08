using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glDrawArrays_d(uint mode, int first, int count);
        private static glDrawArrays_d _glDrawArrays;
        /// <summary>
        /// Render primitives from array data.
        /// </summary>
        /// <param name="mode">Specifies what kind of primitives to render.</param>
        /// <param name="first">Specifies the starting index in the enabled arrays.</param>
        /// <param name="count">Specifies the number of indices to be rendered.</param>
        public static void DrawArrays(PrimitiveType mode, int first, int count)
        {
            _glDrawArrays((uint)mode, first, count);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glDrawElements_d(uint mode, int count, uint type, IntPtr indices);
        private static glDrawElements_d _glDrawElements;
        /// <summary>
        /// Render primitives from array data
        /// </summary>
        /// <param name="mode">Specifies what kind of primitives to render.</param>
        /// <param name="count">Specifies the number of elements to be rendered.</param>
        /// <param name="type">Specifies the type of the values in indices. Must be one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT, or GL_UNSIGNED_INT.</param>
        /// <param name="indices">Specifies a pointer to the location where the indices are stored.</param>
        public static void DrawElements(PrimitiveType mode, int count, DrawElementsType type, IntPtr indices)
        {
            _glDrawElements((uint)mode, count, (uint)type, indices);
        }
    }
}
