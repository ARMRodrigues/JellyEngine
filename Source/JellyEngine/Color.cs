using System;
using System.Numerics;

namespace JellyEngine;

public struct Color
{
    public static Color Jelly => new(0.468f, 0.177f, 0.741f);
    public static Color White => new(1.0f, 1.0f, 1.0f);

    public float R { get; set; }
    public float G { get; set; }
    public float B { get; set; }
    public float A { get; set; }

    public Color(float red, float green, float blue)
    {
       R = red;
       G = green;
       B = blue;
       A = 1.0f;
    }
    
    public Color(float red, float green, float blue, float alpha)
    {
        R = red;
        G = green;
        B = blue;
        A = alpha;
    }

    public readonly Vector3 ToVector3() => new(R, G, B);
    public readonly Vector4 ToVector4() => new(R, G, B, A);
}
