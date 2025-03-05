using System.Numerics;
using JellyEngine;

namespace JellyGame.Scenes.Terrain;

public record Terrain()
{
    public int Seed { get; set; }
    public int Width { get; set; }
    public float Height { get; set; }
    public int Depth { get; set; }
    public Vector2 Offset { get; set; }
    public float NoiseScale { get; set; }
    public int Octaves { get; set; }
    public float Lacunarity { get; set; }
    public float Persistance { get; set; }
    
    private float _shapeStrength = 0.65f;
    public float ShapeStrength
    {
        get => _shapeStrength;
        set => _shapeStrength = MathUtils.Clamp01(value);
    }
}