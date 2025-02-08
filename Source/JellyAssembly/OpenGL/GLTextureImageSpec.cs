using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glTexImage2D_d(
        uint target,
        int level,
        int internalFormat,
        int width,
        int height,
        int border,
        uint format,
        uint type,
        IntPtr data);
        private static glTexImage2D_d _glTexImage2D;

        /// <summary>
        /// Specify a two-dimensional texture image.
        /// </summary>
        /// <param name="target">Specifies the target texture (e.g., GL_TEXTURE_2D).</param>
        /// <param name="level">Specifies the level-of-detail number.</param>
        /// <param name="internalFormat">Specifies the number of color components in the texture.</param>
        /// <param name="width">Specifies the width of the texture image.</param>
        /// <param name="height">Specifies the height of the texture image.</param>
        /// <param name="border">Specifies the width of the border. Must be 0 or 1.</param>
        /// <param name="format">Specifies the format of the pixel data (e.g., GL_RGB).</param>
        /// <param name="type">Specifies the data type of the pixel data (e.g., GL_UNSIGNED_BYTE).</param>
        /// <param name="data">Specifies a pointer to the image data in memory. Can be null for an uninitialized texture.</param>
        public static void TexImage2D(
            TextureTarget target,
            int level,
            PixelInternalFormat internalFormat,
            int width,
            int height,
            int border,
            GLPixelFormat format,
            GLPixelType type,
            IntPtr data)
        {
            _glTexImage2D((uint)target, level, (int)internalFormat, width, height, border, (uint)format, (uint)type, data);
        }

    }
}
