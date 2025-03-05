using System;
using System.Numerics;

namespace JellyEngine;

public struct Color
{
    public static Color Jelly => new(0.468f, 0.177f, 0.741f);
    public static Color Black => new(0.0f, 0.0f, 0.0f);
    public static Color White => new(1.0f, 1.0f, 1.0f);

    public float R { get; set; }
    public float G { get; set; }
    public float B { get; set; }
    public float A { get; set; }

    public Color(float red, float green, float blue)
    {
        R = red > 1.0 ? red / 255f : red;
        G = green > 1.0 ? green / 255f : green;
        B = blue > 1.0 ? blue / 255f : blue;
        A = 1.0f;
    }

    public Color(float red, float green, float blue, float alpha)
    {
        R = red > 1.0 ? red / 255f : red;
        G = green > 1.0 ? green / 255f : green;
        B = blue > 1.0 ? blue / 255f : blue;
        A = alpha;
    }

    public Color(string hex)
    {
        // Remove the hash symbol if present
        hex = hex.TrimStart('#');

        if (hex.Length == 6) // RGB
        {
            R = Convert.ToInt32(hex.Substring(0, 2), 16) / 255f;
            G = Convert.ToInt32(hex.Substring(2, 2), 16) / 255f;
            B = Convert.ToInt32(hex.Substring(4, 2), 16) / 255f;
            A = 1.0f;
        }
        else if (hex.Length == 8) // RGBA
        {
            R = Convert.ToInt32(hex.Substring(0, 2), 16) / 255f;
            G = Convert.ToInt32(hex.Substring(2, 2), 16) / 255f;
            B = Convert.ToInt32(hex.Substring(4, 2), 16) / 255f;
            A = Convert.ToInt32(hex.Substring(6, 2), 16) / 255f;
        }
        else
        {
            throw new ArgumentException("Invalid hex color format. Use #RRGGBB or #RRGGBBAA.");
        }
    }

    public static Color operator +(Color c1, Color c2)
    {
        return new Color(c1.R + c2.R, c1.G + c2.G, c1.B + c2.B, c1.A + c2.A);
    }

    public static Color operator /(Color c, float scalar)
    {
        return new Color(c.R / scalar, c.G / scalar, c.B / scalar, c.A / scalar);
    }

    public readonly Vector3 ToVector3() => new(R, G, B);
    public readonly Vector4 ToVector4() => new(R, G, B, A);
    public static Color Lerp(Color a, Color b, float t)
    {
        return new Color(
            a.R + (b.R - a.R) * t,
            a.G + (b.G - a.G) * t,
            a.B + (b.B - a.B) * t,
            a.A + (b.A - a.A) * t
        );
    }
}
