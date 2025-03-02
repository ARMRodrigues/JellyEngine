/// Based on: https://github.com/TheThinMatrix/LowPolyTerrain

using System;
using JellyEngine;

namespace JellyGame.Scenes.Terrain;

public class ColorGenerator
{
    private readonly float _spread;
    private readonly float _halfSpread;
    private readonly Color[] _biomeColors;
    private readonly float _part;

    public ColorGenerator(Color[] biomeColor, float spread)
    {
        _biomeColors = biomeColor;
        _spread = spread;
        _halfSpread = spread / 2f;
        _part = 1f / (biomeColor.Length - 1);
    }

    public Color[,] GenerateColours(float[,] heights, float amplitude)
    {
        int size = heights.GetLength(0);
        int height = heights.GetLength(1);
        var colours = new Color[size, height];

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < size; x++)
            {
                colours[x, z] = CalculateColour(heights[x, z], amplitude);
            }
        }
        return colours;
    }

    private Color CalculateColour(float height, float amplitude)
    {
        float value = (height + amplitude) / (amplitude * 2);
        value = Math.Clamp((value - _halfSpread) * (1f / _spread), 0f, 0.9999f);

        int firstBiome = (int)Math.Floor(value / _part);
        float blend = (value - (firstBiome * _part)) / _part;

        return Color.Lerp(_biomeColors[firstBiome], _biomeColors[firstBiome + 1], blend);
    }
}
