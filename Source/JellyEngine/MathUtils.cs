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

    // Clamp: Restringe um valor entre um mínimo e um máximo
    public static float Clamp(float value, float min, float max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    // Min: Retorna o menor valor entre dois números
    public static float Min(float a, float b)
    {
        return a < b ? a : b;
    }

    // Max: Retorna o maior valor entre dois números
    public static float Max(float a, float b)
    {
        return a > b ? a : b;
    }

    // Abs: Retorna o valor absoluto de um número
    public static float Abs(float value)
    {
        return value < 0 ? -value : value;
    }

    // Sqrt: Retorna a raiz quadrada de um número (usando o método de Newton)
    public static float Sqrt(float value)
    {
        if (value < 0) throw new System.ArgumentOutOfRangeException("Não é possível calcular a raiz quadrada de um número negativo.");

        float guess = value;
        float tolerance = 1e-7f; // Precisão desejada

        while (Abs(guess * guess - value) > tolerance)
        {
            guess = (guess + value / guess) / 2.0f;
        }

        return guess;
    }

    // Sin: Retorna o seno de um ângulo em radianos (usando série de Taylor)
    public static float Sin(float angle)
    {
        // Normaliza o ângulo para o intervalo [0, 2π]
        angle = angle % (2 * MathF.PI);
        if (angle < 0) angle += 2 * MathF.PI;

        float result = 0;
        float term = angle;
        int n = 1;

        // Série de Taylor para seno
        for (int i = 0; i < 10; i++) // 10 iterações para precisão razoável
        {
            result += term;
            term *= -(angle * angle) / ((2 * n) * (2 * n + 1));
            n++;
        }

        return result;
    }

    // Atan2: Retorna o ângulo cuja tangente é o quociente de dois números especificados
    public static float Atan2(float y, float x)
    {
        if (x == 0)
        {
            if (y > 0) return MathF.PI / 2;
            if (y < 0) return -MathF.PI / 2;
            return 0; // Indefinido, mas retorna 0 por convenção
        }

        float atan = MathF.Atan(Abs(y / x));

        if (x > 0 && y >= 0) return atan; // Primeiro quadrante
        if (x > 0 && y < 0) return -atan; // Quarto quadrante
        if (x < 0 && y >= 0) return MathF.PI - atan; // Segundo quadrante
        return -(MathF.PI - atan); // Terceiro quadrante
    }

    // Cos: Retorna o cosseno de um ângulo em radianos (usando série de Taylor)
    public static float Cos(float angle)
    {
        // Normaliza o ângulo para o intervalo [0, 2π]
        angle = angle % (2 * MathF.PI);
        if (angle < 0) angle += 2 * MathF.PI;

        float result = 0;
        float term = 1;
        int n = 0;

        // Série de Taylor para cosseno
        for (int i = 0; i < 10; i++) // 10 iterações para precisão razoável
        {
            result += term;
            term *= -(angle * angle) / ((2 * n + 1) * (2 * n + 2));
            n++;
        }

        return result;
    }

    // Lerp: Interpola linearmente entre dois valores
    public static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * Clamp(t, 0, 1);
    }

    // Tan: Retorna a tangente de um ângulo em radianos
    public static float Tan(float angle)
    {
        return Sin(angle) / Cos(angle);
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