using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glClear_d(uint mask);
        private static glClear_d _glClear;
        /// <summary>
        /// Clear buffers to preset values
        /// </summary>
        /// <param name="mask">Bitwise OR of masks that indicate the buffers to be cleared. The three masks are GL_COLOR_BUFFER_BIT, GL_DEPTH_BUFFER_BIT, and GL_STENCIL_BUFFER_BIT.</param>
        public static void Clear(ClearBufferMask mask)
        {
            _glClear((uint)mask);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glClearColor_d(float red, float green, float blue, float alpha);
        private static glClearColor_d _glClearColor;
        /// <summary>
        /// Specify clear values for the color buffers.
        /// </summary>
        /// <param name="red">Specify the red value used when the color buffers are cleared. The initial values are all 0.</param>
        /// <param name="green">Specify the green value used when the color buffers are cleared. The initial values are all 0.</param>
        /// <param name="blue">Specify blue value used when the color buffers are cleared. The initial values are all 0.</param>
        /// <param name="alpha">Specify alpha value used when the color buffers are cleared. The initial values are all 0.</param>
        public static void ClearColor(float red, float green, float blue, float alpha)
        {
            _glClearColor(red, green, blue, alpha);
        }


    }
}
