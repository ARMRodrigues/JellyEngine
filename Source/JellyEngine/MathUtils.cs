using System.Numerics;

namespace JellyEngine;

public struct MathUtils
{
    public static float ToRadians(float degrees)
    {
        return degrees * (MathF.PI / 180f);
    }

    public static float ToDegrees(float radians)
    {
        return radians * (180f / MathF.PI);
    }
    
    public static float[] ToOpenGLMatrixArray(Matrix4x4 matrix)
    {
        return
        [
            matrix.M11, matrix.M12, matrix.M13, matrix.M14,
            matrix.M21, matrix.M22, matrix.M23, matrix.M24,
            matrix.M31, matrix.M32, matrix.M33, matrix.M34,
            matrix.M41, matrix.M42, matrix.M43, matrix.M44
        ];
    }
}