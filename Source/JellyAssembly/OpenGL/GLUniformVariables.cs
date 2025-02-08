using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int glGetUniformLocation_d(uint program, string name);
        private static glGetUniformLocation_d _glGetUniformLocation;
        /// <summary>
        /// Returns the location of a uniform variable
        /// </summary>
        /// <param name="program">Specifies the program object to be queried.</param>
        /// <param name="name">Points to a null terminated string containing the name of the uniform variable whose location is to be queried.</param>
        /// <returns>Returns an integer that represents the location of a specific uniform variable within a the default uniform block of a program object.</returns>
        public static int GetUniformLocation(uint program, string name)
        {
            int location = _glGetUniformLocation(program, name);
            return location;
        }
    }
}
