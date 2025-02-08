using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glGenTextures_d(uint n, uint[] textures);
        private static glGenTextures_d _glGenTextures;

        /// <summary>
        /// Generate a single texture object name.
        /// </summary>
        /// <returns>The ID of the generated texture object.</returns>
        public static uint GenTexture()
        {
            uint[] textures = new uint[1];
            _glGenTextures(1, textures);
            return textures[0];
        }

        /// <summary>
        /// Generate multiple texture object names.
        /// </summary>
        /// <param name="n">Specifies the number of texture object names to generate.</param>
        /// <returns>An array containing the generated texture object names.</returns>
        public static uint[] GenTextures(uint n)
        {
            uint[] textures = new uint[n];
            _glGenTextures(n, textures);
            return textures;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glBindTexture_d(uint target, uint texture);
        private static glBindTexture_d _glBindTexture;

        /// <summary>
        /// Bind a named texture to a texturing target.
        /// </summary>
        /// <param name="target">Specifies the target to which the texture is bound (e.g., GL_TEXTURE_2D).</param>
        /// <param name="texture">Specifies the name of the texture.</param>
        public static void BindTexture(TextureTarget target, uint texture)
        {
            _glBindTexture((uint)target, texture);
        }
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glDeleteTextures_d(uint n, uint[] textures);
        private static glDeleteTextures_d _glDeleteTextures;

        /// <summary>
        /// Delete texture objects.
        /// </summary>
        /// <param name="texture">The ID of the texture to delete.</param>
        public static void DeleteTexture(uint texture)
        {
            _glDeleteTextures(1, new uint[] { texture });
        }

        /// <summary>
        /// Delete multiple texture objects.
        /// </summary>
        /// <param name="textures">An array containing the IDs of textures to delete.</param>
        public static void DeleteTextures(uint[] textures)
        {
            _glDeleteTextures((uint)textures.Length, textures);
        }

    }
}
