using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL;

public partial class GL
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glGetTexImage_d(
        uint target, 
        int level, 
        uint format, 
        uint type, 
        IntPtr pixels);
    private static glGetTexImage_d _glGetTexImage;

    /// <summary>
    /// Retrieves the texture image.
    /// </summary>
    /// <param name="target">Specifies the target texture (e.g., GL_TEXTURE_2D).</param>
    /// <param name="level">Specifies the level-of-detail number of the desired image.</param>
    /// <param name="format">Specifies the format of the pixel data (e.g., GL_RGBA).</param>
    /// <param name="type">Specifies the data type of the pixel data (e.g., GL_UNSIGNED_BYTE).</param>
    /// <param name="pixels">Pointer to the buffer where the texture image will be stored.</param>
    public static void GetTexImage(
        TextureTarget target, 
        int level, 
        GLPixelFormat format, 
        GLPixelType type, 
        IntPtr pixels)
    {
        _glGetTexImage((uint)target, level, (uint)format, (uint)type, pixels);
    }
}