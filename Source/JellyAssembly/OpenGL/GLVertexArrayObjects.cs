using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glGenVertexArrays_d(uint n, uint[] arrays);
        private static glGenVertexArrays_d _glGenVertexArrays;
        /// <summary>
        /// Generate vertex array object names.
        /// </summary>
        /// <returns>The id of the array in which the generated vertex array object names are stored.</returns>
        public static uint GenVertexArrays()
        {
            uint[] arrays = new uint[1];
            _glGenVertexArrays(1, arrays);
            return arrays[0];
        }
        /// <summary>
        /// Generate vertex array object names.
        /// </summary>
        /// <param name="size">Specifies the number of vertex array object names to generate.</param>
        /// <returns>The id of the array in which the generated vertex array object names are stored.</returns>
        public static uint[] GenVertexArrays(uint n)
        {
            uint[] arrays = new uint[n];
            _glGenVertexArrays(n, arrays);
            return arrays;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glDeleteVertexArrays_d(int n, ref uint arrays);
        private static glDeleteVertexArrays_d _glDeleteVertexArrays;
        /// <summary>
        /// Delete vertex array objects.
        /// </summary>
        /// <param name="size">Specifies the number of vertex array objects to be deleted.</param>
        /// <param name="vaoId">Specifies the address of an array containing the n names of the objects to be deleted.</param>
        public static void DeleteVertexArrays(int size, uint vaoId)
        {
            _glDeleteVertexArrays(size, ref vaoId);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glBindVertexArray_d(uint array);
        private static glBindVertexArray_d _glBindVertexArray;
        /// <summary>
        /// Bind a vertex array object.
        /// </summary>
        /// <param name="array">Specifies the name of the vertex array to bind.</param>
        public static void BindVertexArray(uint array)
        {
            _glBindVertexArray(array);
        }
    }
}
