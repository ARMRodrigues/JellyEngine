using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glBufferData_d(uint target, IntPtr size, IntPtr data, uint usage);
        private static glBufferData_d _glBufferData;
        /// <summary>
        ///  Creates and initializes a buffer object's data store.
        /// </summary>
        /// <param name="target">Specifies the target to which the buffer object is bound for glBufferData, which must be one of the buffer binding targets in the following BufferTarget enum</param>
        /// <param name="size">Specifies the size in bytes of the buffer object's new data store.</param>
        /// <param name="data">Specifies a pointer to data that will be copied into the data store for initialization, or NULL if no data is to be copied.</param>
        /// <param name="usage">Specifies the expected usage pattern of the data store.</param>
        public static void BufferData(BufferTarget target, int size, float[] data, BufferUsageHint usage)
        {
            var sizePtr = (IntPtr)(size * sizeof(float));
            unsafe
            {
                fixed (float* dataPtr = data)
                {
                    _glBufferData((uint)target, sizePtr, (IntPtr)dataPtr, (uint)usage);
                }
            }
        }
        /// <summary>
        ///  Creates and initializes a buffer object's data store.
        /// </summary>
        /// <param name="target">Specifies the target to which the buffer object is bound for glBufferData, which must be one of the buffer binding targets in the following BufferTarget enum</param>
        /// <param name="size">Specifies the size in bytes of the buffer object's new data store.</param>
        /// <param name="data">Specifies a pointer to data that will be copied into the data store for initialization, or NULL if no data is to be copied.</param>
        /// <param name="usage">Specifies the expected usage pattern of the data store.</param>
        public static void BufferData(BufferTarget target, int size, uint[] data, BufferUsageHint usage)
        {
            var sizePtr = (IntPtr)(size * sizeof(float));
            unsafe
            {
                fixed (uint* dataPtr = data)
                {
                    _glBufferData((uint)target, sizePtr, (IntPtr)dataPtr, (uint)usage);
                }
            }
        }
    }
}
