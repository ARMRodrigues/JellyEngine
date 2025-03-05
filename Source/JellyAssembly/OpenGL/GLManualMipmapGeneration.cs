using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL;

public partial class GL
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void glGenerateMipmap_d(uint target);
    private static glGenerateMipmap_d _glGenerateMipmap;

    /// <summary>
    /// Generates mipmaps for the specified texture target.
    /// </summary>
    /// <param name="target">Specifies the target texture (e.g., GL_TEXTURE_2D).</param>
    public static void GenerateMipmap(TextureTarget target)
    {
        _glGenerateMipmap((uint)target);
    }
}