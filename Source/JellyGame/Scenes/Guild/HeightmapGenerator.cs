using System;
using System.Numerics;
using JellyEngine;

namespace JellyGame.Scenes.Guild;

public class HeightmapGenerator
{
    public readonly float[,] HeightMap;
    public readonly float[,] FallOffMap;
    public readonly float[,] BiomeMap;

    private readonly int _width;
    private readonly int _height;
    private FastNoiseLite _noise;
    
    float _maxNoiseHeight = float.MinValue;
    float _minNoiseHeight = float.MaxValue;
    
    int _seed = 0;

    public HeightmapGenerator(int width, int length)
    {
        _width = width;
        _height = length;
        //_seed = terrain.Seed;
        
        _noise = new FastNoiseLite(_seed);
        _noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
        _noise.SetFrequency(3f);
        _noise.SetFractalType(FastNoiseLite.FractalType.None);
       
        HeightMap = new float[_width, _height];
        FallOffMap = new float[_width, _height];
        BiomeMap = new float[_width, _height];

        GenerateHeightmap();
        GenerateFalloffMap();
    }

    private void GenerateHeightmap()
    {
        var scale = 250f;
        var octaves = 5;
        var persistance = 0.5f;
        var lacunarity = 0.4f;

        var prng = new System.Random(_seed);
        var octavesOffsets = new Vector2[octaves];

        var offset = new Vector2();
        
        for (var i = 0; i < octaves; i++)
        {
            var offsetX = prng.Next(-100000, 100000) + offset.X/2;
            var offsetY = prng.Next(-100000, 100000) + offset.Y/2;
            octavesOffsets[i] = new Vector2(offsetX, offsetY);
        }
        
        var halfWidth = _width / 2;
        var halfHeight = _height / 2;
        
        var aspect = _width / (float)_height;

        for (int z = 0; z < _height; z++)
        {
            for (int x = 0; x < _width; x++)
            {
                var amplitude = 1f;
                var frequency = 1f;
                var noiseHeight = 0f;
                
                
                for (var i = 0; i < octaves; i++)
                {
                    var sampleX = (x - halfWidth) / scale * frequency * aspect + octavesOffsets[i].X * frequency;
                    var sampleZ = (z - halfHeight) / scale * frequency * aspect + octavesOffsets[i].Y * frequency;
                    
                    var baseHeight = _noise.GetNoise(sampleX, sampleZ);
                    //var baseHeight = Mathf.PerlinNoise(sampleX, sampleZ);
                    noiseHeight += baseHeight * amplitude;
                    
                    amplitude *= persistance;
                    frequency /= lacunarity;
                }
                
                /*var nx = x/(float)_width -0.5f;
                var ny = z/(float)_height -0.5f;

                var e = (1.0f) * anotherNoise(1 * nx, 1 * ny)
                            + (0.50f) * anotherNoise(2 * nx, 2 * ny)
                            + (0.25f) * anotherNoise(4 * nx, 4 * ny)
                            + (0.13f) * anotherNoise(8 * nx, 8 * ny)
                            + (0.06f) * anotherNoise(16 * nx, 16 * ny)
                            + (0.03f) * anotherNoise(32 * nx, 32 * ny);
                e = e / (1.0f + 0.5f + 0.25f + 0.13f + 0.06f + 0.03f);
                e = (float)Math.Pow(e, 1.5f);
                
                
                var m = (1.0f) * biomeNoiseGen(1 * nx, 1 * ny)
                        + (0.75f) * biomeNoiseGen(2 * nx, 2 * ny)
                        + (0.33f) * biomeNoiseGen(4 * nx, 4 * ny)
                        + (0.33f) * biomeNoiseGen(8 * nx, 8 * ny)
                        + (0.33f) * biomeNoiseGen(16 * nx, 16 * ny)
                        + (0.50f) * biomeNoiseGen(32 * nx, 32 * ny);
                m = m / (1.0f + 0.75f + 0.33f + 0.33f + 0.33f + 0.5f);*/
                
                if (noiseHeight > _maxNoiseHeight)
                {
                    _maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight <= _minNoiseHeight)
                {
                    _minNoiseHeight = noiseHeight;
                }

                HeightMap[x, z] = noiseHeight;
                //BiomeMap[x, z] = m;
            }
        }

        for (var z = 0; z < _height; z++)
        {
            for (var x = 0; x < _width; x++)
            {
                HeightMap[x, z] = MathUtils.InverseLerp(_minNoiseHeight, _maxNoiseHeight, HeightMap[x, z]);
            }
        }
    }

    private void GenerateFalloffMap()
    {
        for (int z = 0; z < _height; z++)
        {
            for (int x = 0; x < _width; x++)
            {
                var nx = 2f * (x / (float)_width) - 1;
                var ny = 2f * (z / (float)_height) - 1;
                
                var d = Distance(nx, ny, DistanceFunctions.SquareBump);
                
                if (d < 0) d = 0;
                if (d > 1) d = 1;
                
                var e = HeightMap[x, z];
                e = Reshape(e, d);

                FallOffMap[x, z] = e;
                BiomeMap[x, z] = Reshape(BiomeMap[x, z], d);
            }
        }
    }
    
    private static float Distance(float nx, float ny, DistanceFunctions funcType)
    {
        switch (funcType)
        {
            case DistanceFunctions.SquareBump:
                return 1 - (1 - nx * nx) * (1 - ny * ny);
            case DistanceFunctions.EuclideanSquared:
                return MathF.Min(1f, (nx * nx + ny * ny) / (float)MathUtils.Sqrt(2));
            case DistanceFunctions.Diagonal:
                return MathUtils.Max(MathUtils.Abs(nx), MathUtils.Abs(ny));
            case DistanceFunctions.Manhattan:
                return (MathUtils.Abs(nx) + MathUtils.Abs(ny)) / 2f;
            case DistanceFunctions.Euclidean:
                return (float)MathUtils.Sqrt(nx * nx + ny * ny) / (float)MathUtils.Sqrt(2f);
            case DistanceFunctions.Hyperboloid:
                return (float)(MathUtils.Sqrt(nx * nx + ny * ny + 0.2f * 0.2f) - 0.2f) /
                       (float)(MathUtils.Sqrt(1 * 1 + 1 * 1 + 0.2f * 0.2f) - 0.2f);
            case DistanceFunctions.Blob:
                return (float)(MathUtils.Sqrt(nx * nx + (ny - 0.05f) * (ny - 0.05f)) * MathUtils.Sqrt(2f) * 2.7f /
                       (3 - MathUtils.Sin(5 * MathUtils.Atan2(ny - 0.05f, nx))));
            default:
                return 1 - (1 - nx * nx) * (1 - ny * ny);
        }
    }

    private static float Reshape(float e, float d)
    {
        return MathUtils.Lerp(e, 1 - d, 0.65f);
    }
}