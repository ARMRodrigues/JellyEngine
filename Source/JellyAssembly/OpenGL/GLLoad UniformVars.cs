using System.Numerics;
using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
    #pragma warning disable CS8618
    public partial class GL
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glUniform1f_d(int location, float v0);
        private static glUniform1f_d _glUniform1f;
        /// <summary>
        /// Specify the value of a uniform variable for the current program object
        /// </summary>
        /// <param name="location">Specifies the location of the uniform variable to be modified.</param>
        /// <param name="value"> The value you want to set for the uniform.</param>
        public static void Uniform1f(int location, float value)
        {
            _glUniform1f(location, value);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glUniform3f_d(int location, float v0, float v1, float v2);
        private static glUniform3f_d _glUniform3f;

        /// <summary>
        /// Specify the value of a vec3 uniform variable for the current program object.
        /// </summary>
        /// <param name="location">Specifies the location of the uniform variable to be modified.</param>
        /// <param name="v0">The first component of the vector.</param>
        /// <param name="v1">The second component of the vector.</param>
        /// <param name="v2">The third component of the vector.</param>
        public static void Uniform3f(int location, float v0, float v1, float v2)
        {
            _glUniform3f(location, v0, v1, v2);
        }

        /// <summary>
        /// Specify the value of a vec3 uniform variable for the current program object using a Vector3.
        /// </summary>
        /// <param name="location">Specifies the location of the uniform variable to be modified.</param>
        /// <param name="vector">The vector containing the values to set.</param>
        public static void Uniform3f(int location, Vector3 vector)
        {
            _glUniform3f(location, vector.X, vector.Y, vector.Z);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glUniformMatrix4fv_d(int location, int count, bool transpose, float[] value);
        private static glUniformMatrix4fv_d _glUniformMatrix4fv;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location">Specifies the location of the uniform variable to be modified.</param>
        /// <param name="count">Specifies the number of matrices that are to be modified. This should be 1 if the targeted uniform variable is not an array of matrices, and 1 or more if it is an array of matrices.</param>
        /// <param name="transpose">Specifies whether to transpose the matrix as the values are loaded into the uniform variable.</param>
        /// <param name="value">Specifies a pointer to an array of count values that will be used to update the specified uniform variable.</param>
        public static void UniformMatrix4fv(int location, int count, bool transpose, float[] value)
        {
            _glUniformMatrix4fv(location, count, transpose, value);
        }
    }
}
