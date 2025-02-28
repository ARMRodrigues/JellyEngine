using System;

namespace JellyEngine;

public class Noise
{
    private record Grad(float X, float Y, float Z)
    {
        public float Dot2(float x, float y) => X * x + Y * y;
        public float Dot3(float x, float y, float z) => X * x + Y * y + Z * z;
    }

    private static readonly Grad[] Grad3 = {
        new(1f, 1f, 0f), new(-1f, 1f, 0f), new(1f, -1f, 0f), new(-1f, -1f, 0f),
        new(1f, 0f, 1f), new(-1f, 0f, 1f), new(1f, 0f, -1f), new(-1f, 0f, -1f),
        new(0f, 1f, 1f), new(0f, -1f, 1f), new(0f, 1f, -1f), new(0f, -1f, -1f)
    };

    private readonly int[] _p = {
        151, 160, 137, 91, 90, 15, 131, 13, 201, 95, 96, 53, 194, 233, 7, 225, 140, 36, 103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23,
        190, 6, 148, 247, 120, 234, 75, 0, 26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 57, 177, 33, 88, 237, 149, 56, 87, 174, 20, 125, 136, 171, 168, 68, 175, 74, 165, 71, 134, 139, 48, 27, 166,
        77, 146, 158, 231, 83, 111, 229, 122, 60, 211, 133, 230, 220, 105, 92, 41, 55, 46, 245, 40, 244, 102, 143, 54, 65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132, 187, 208, 89, 18, 169, 200, 196,
        135, 130, 116, 188, 159, 86, 164, 100, 109, 198, 173, 186, 3, 64, 52, 217, 226, 250, 124, 123, 5, 202, 38, 147, 118, 126, 255, 82, 85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189, 28, 42,
        223, 183, 170, 213, 119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101, 155, 167, 43, 172, 9, 129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232, 178, 185, 112, 104, 218, 246, 97, 228, 251, 34, 242, 193, 238, 210, 144, 12, 191, 179, 162, 241, 81, 51, 145, 235, 249, 14, 239, 107,
        49, 192, 214, 31, 181, 199, 106, 157, 184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254, 138, 236, 205, 93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215, 61, 156, 180
    };

    private readonly int[] _perm = new int[512];
    private readonly Grad[] _gradP = new Grad[512];

    private float F2 = 0.5f * ((float)Math.Sqrt(3) - 1f);
    private float G2 = (3f - (float)Math.Sqrt(3)) / 6f;
    private float F3 = 1.0f / 3;
    private float G3 = 1.0f / 6;

    public Noise(int seed = 0)
    {
        Seed(seed);
    }

    public void Seed(int seed)
    {
        if (seed > 0 && seed < 1)
        {
            seed = (int)(seed * 65536);
        }

        seed = (int)Math.Floor((float)seed);
        if (seed < 256)
        {
            seed |= seed << 8;
        }

        for (int i = 0; i < 256; i++)
        {
            int v = (i & 1) == 1 ? _p[i] ^ (seed & 255) : _p[i] ^ ((seed >> 8) & 255);
            _perm[i] = _perm[i + 256] = v;
            _gradP[i] = _gradP[i + 256] = Grad3[v % 12];
        }
    }

    public float Simplex2(float xin, float yin)
    {
        float n0, n1, n2;

        float s = (xin + yin) * F2;
        int i = (int)Math.Floor(xin + s);
        int j = (int)Math.Floor(yin + s);
        float t = (i + j) * G2;
        float x0 = xin - i + t;
        float y0 = yin - j + t;

        int i1, j1;
        if (x0 > y0)
        {
            i1 = 1; j1 = 0;
        }
        else
        {
            i1 = 0; j1 = 1;
        }

        float x1 = x0 - i1 + G2;
        float y1 = y0 - j1 + G2;
        float x2 = x0 - 1 + 2 * G2;
        float y2 = y0 - 1 + 2 * G2;

        i &= 255;
        j &= 255;

        Grad gi0 = _gradP[i + _perm[j]];
        Grad gi1 = _gradP[i + i1 + _perm[j + j1]];
        Grad gi2 = _gradP[i + 1 + _perm[j + 1]];

        float t0 = 0.5f - x0 * x0 - y0 * y0;
        if (t0 < 0)
        {
            n0 = 0;
        }
        else
        {
            t0 *= t0;
            n0 = t0 * t0 * gi0.Dot2(x0, y0);
        }

        float t1 = 0.5f - x1 * x1 - y1 * y1;
        if (t1 < 0)
        {
            n1 = 0;
        }
        else
        {
            t1 *= t1;
            n1 = t1 * t1 * gi1.Dot2(x1, y1);
        }

        float t2 = 0.5f - x2 * x2 - y2 * y2;
        if (t2 < 0)
        {
            n2 = 0;
        }
        else
        {
            t2 *= t2;
            n2 = t2 * t2 * gi2.Dot2(x2, y2);
        }

        return 70 * (n0 + n1 + n2);
    }

    public float Simplex3(float xin, float yin, float zin)
    {
        float n0, n1, n2, n3;

        float s = (xin + yin + zin) * F3;
        int i = (int)Math.Floor(xin + s);
        int j = (int)Math.Floor(yin + s);
        int k = (int)Math.Floor(zin + s);

        float t = (i + j + k) * G3;
        float x0 = xin - i + t;
        float y0 = yin - j + t;
        float z0 = zin - k + t;

        int i1, j1, k1;
        int i2, j2, k2;

        if (x0 >= y0)
        {
            if (y0 >= z0) { i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 1; k2 = 0; }
            else if (x0 >= z0) { i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 0; k2 = 1; }
            else { i1 = 0; j1 = 0; k1 = 1; i2 = 1; j2 = 0; k2 = 1; }
        }
        else
        {
            if (y0 < z0) { i1 = 0; j1 = 0; k1 = 1; i2 = 0; j2 = 1; k2 = 1; }
            else if (x0 < z0) { i1 = 0; j1 = 1; k1 = 0; i2 = 0; j2 = 1; k2 = 1; }
            else { i1 = 0; j1 = 1; k1 = 0; i2 = 1; j2 = 1; k2 = 0; }
        }

        float x1 = x0 - i1 + G3;
        float y1 = y0 - j1 + G3;
        float z1 = z0 - k1 + G3;

        float x2 = x0 - i2 + 2 * G3;
        float y2 = y0 - j2 + 2 * G3;
        float z2 = z0 - k2 + 2 * G3;

        float x3 = x0 - 1 + 3 * G3;
        float y3 = y0 - 1 + 3 * G3;
        float z3 = z0 - 1 + 3 * G3;

        i &= 255;
        j &= 255;
        k &= 255;

        Grad gi0 = _gradP[i + _perm[j + _perm[k]]];
        Grad gi1 = _gradP[i + i1 + _perm[j + j1 + _perm[k + k1]]];
        Grad gi2 = _gradP[i + i2 + _perm[j + j2 + _perm[k + k2]]];
        Grad gi3 = _gradP[i + 1 + _perm[j + 1 + _perm[k + 1]]];

        float t0 = 0.5f - x0 * x0 - y0 * y0 - z0 * z0;
        if (t0 < 0)
        {
            n0 = 0;
        }
        else
        {
            t0 *= t0;
            n0 = t0 * t0 * gi0.Dot3(x0, y0, z0);
        }

        float t1 = 0.5f - x1 * x1 - y1 * y1 - z1 * z1;
        if (t1 < 0)
        {
            n1 = 0;
        }
        else
        {
            t1 *= t1;
            n1 = t1 * t1 * gi1.Dot3(x1, y1, z1);
        }

        float t2 = 0.5f - x2 * x2 - y2 * y2 - z2 * z2;
        if (t2 < 0)
        {
            n2 = 0;
        }
        else
        {
            t2 *= t2;
            n2 = t2 * t2 * gi2.Dot3(x2, y2, z2);
        }

        float t3 = 0.5f - x3 * x3 - y3 * y3 - z3 * z3;
        if (t3 < 0)
        {
            n3 = 0;
        }
        else
        {
            t3 *= t3;
            n3 = t3 * t3 * gi3.Dot3(x3, y3, z3);
        }

        return 32 * (n0 + n1 + n2 + n3);
    }

    private static float Fade(float t) => t * t * t * (t * (t * 6 - 15) + 10);

    private static float Lerp(float a, float b, float t) => (1 - t) * a + t * b;

    public float Perlin2(float x, float y)
    {
        int X = (int)Math.Floor(x);
        int Y = (int)Math.Floor(y);
        x -= X;
        y -= Y;
        X &= 255;
        Y &= 255;

        Grad g00 = _gradP[X + _perm[Y]];
        Grad g01 = _gradP[X + _perm[Y + 1]];
        Grad g10 = _gradP[X + 1 + _perm[Y]];
        Grad g11 = _gradP[X + 1 + _perm[Y + 1]];

        float n00 = g00.Dot2(x, y);
        float n01 = g01.Dot2(x, y - 1);
        float n10 = g10.Dot2(x - 1, y);
        float n11 = g11.Dot2(x - 1, y - 1);

        float u = Fade(x);
        return Lerp(Lerp(n00, n10, u), Lerp(n01, n11, u), Fade(y));
    }

    public float Perlin3(float x, float y, float z)
    {
        int X = (int)Math.Floor(x);
        int Y = (int)Math.Floor(y);
        int Z = (int)Math.Floor(z);
        x -= X;
        y -= Y;
        z -= Z;
        X &= 255;
        Y &= 255;
        Z &= 255;

        Grad g000 = _gradP[X + _perm[Y + _perm[Z]]];
        Grad g001 = _gradP[X + _perm[Y + _perm[Z + 1]]];
        Grad g010 = _gradP[X + _perm[Y + 1 + _perm[Z]]];
        Grad g011 = _gradP[X + _perm[Y + 1 + _perm[Z + 1]]];
        Grad g100 = _gradP[X + 1 + _perm[Y + _perm[Z]]];
        Grad g101 = _gradP[X + 1 + _perm[Y + _perm[Z + 1]]];
        Grad g110 = _gradP[X + 1 + _perm[Y + 1 + _perm[Z]]];
        Grad g111 = _gradP[X + 1 + _perm[Y + 1 + _perm[Z + 1]]];

        float n000 = g000.Dot3(x, y, z);
        float n001 = g001.Dot3(x, y, z - 1);
        float n010 = g010.Dot3(x, y - 1, z);
        float n011 = g011.Dot3(x, y - 1, z - 1);
        float n100 = g100.Dot3(x - 1, y, z);
        float n101 = g101.Dot3(x - 1, y, z - 1);
        float n110 = g110.Dot3(x - 1, y - 1, z);
        float n111 = g111.Dot3(x - 1, y - 1, z - 1);

        float u = Fade(x);
        float v = Fade(y);
        float w = Fade(z);

        return Lerp(
            Lerp(Lerp(n000, n100, u), Lerp(n001, n101, u), w),
            Lerp(Lerp(n010, n110, u), Lerp(n011, n111, u), w),
            v);
    }
}