using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glViewport_d(int x, int y, int width, int height);
        private static glViewport_d _glViewport;

        /// <summary>
        /// Set the viewport.
        /// </summary>
        /// <param name="x">Specifies the lower-left corner of the viewport rectangle, in pixels (X coordinate).</param>
        /// <param name="y">Specifies the lower-left corner of the viewport rectangle, in pixels (Y coordinate).</param>
        /// <param name="width">Specifies the width of the viewport.</param>
        /// <param name="height">Specifies the height of the viewport.</param>
        public static void Viewport(int x, int y, int width, int height)
        {
            _glViewport(x, y, width, height);
        }

    }
}