using System.Numerics;
using JellyEngine;

namespace JellyGame.Scenes.Terrain;

public class HeightmapGenerator
{
    public enum DISTANCE_FUNCTIONS
    {
        SquareBump,
        EuclideanSquared,
        Diagonal,
        Manhattan,
        Euclidean,
        Hyperboloid,
        Blob
    }

    public readonly float[,] HeightMap;
    public readonly float[,] FallOffMap;

    //private readonly int _size;
    private readonly int _width;
    private readonly int _height;
    private readonly FastNoiseLite _noise;
    
    float maxNoiseHeight = float.MinValue;
    float minNoiseHeight = float.MaxValue;

    public HeightmapGenerator(int width, int height, FastNoiseLite noise)
    {
        //_size = newSize + 1;
        _width = width + 1;
        _height = height + 1;
        _noise = noise;
       
        HeightMap = new float[_width, _height];
        FallOffMap = new float[_width, _height];

        GenerateHeightmap();
    }

    public void GenerateHeightmap()
    {
        FastNoiseLite detailNoise = new FastNoiseLite();
        detailNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        detailNoise.SetFrequency(-0.020f);
        detailNoise.SetFractalType(FastNoiseLite.FractalType.None);
        detailNoise.SetCellularDistanceFunction(FastNoiseLite.CellularDistanceFunction.EuclideanSq);
        detailNoise.SetDomainWarpType(FastNoiseLite.DomainWarpType.OpenSimplex2);
        detailNoise.SetDomainWarpAmp(70);
        detailNoise.SetFractalOctaves(4);

        //float detailHeight = detailNoise.GetNoise(x * 0.3f, z * 0.3f) * 2f;

        var noise = new Noise();
        noise.Seed(new Random().Next(0, 1000000));

        var scale = 30f;
        var octaves = 3;
        var persistance = 0.5f;
        var lacunarity = 2f;
        var seed = 4456864;

        var prng = new Random(seed);
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
                    var sampleX = (x - halfWidth + octavesOffsets[i].X) / scale * frequency * aspect;
                    var sampleZ = (z - halfHeight + octavesOffsets[i].Y) / scale * frequency * aspect;
                    
                    var baseHeight = noise.Perlin2(sampleX, sampleZ);
                    noiseHeight += baseHeight * amplitude;
                    
                    //noiseHeight = ridgenoise(noiseHeight);
                    
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                
                //float detailHeight = detailNoise.GetNoise(x * 0.3f, z * 0.3f) * 2f;
                
                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight <= minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                //noiseHeight = (noiseHeight + 1) * 0.5f;

                var value = _noise.GetNoise(x, z);
                
                value = (value + 1) * 0.5f;

                HeightMap[x, z] = noiseHeight;
            }
        }

        for (var z = 0; z < _height; z++)
        {
            for (var x = 0; x < _width; x++)
            {
                HeightMap[x, z] = MathUtils.InverseLerp(minNoiseHeight, maxNoiseHeight, HeightMap[x, z]);
                //HeightMap[x, z] = NewNoiseRange(HeightMap[x, z]);
            }
        }
        var min = float.MaxValue;
        var max = float.MinValue;
        for (var z = 0; z < _height; z++)
        {
            for (var x = 0; x < _width; x++)
            {
                var value = HeightMap[x, z];
                if (value < min) min = value;
                if (value > max) max = value;
            }
        }

        for (int z = 0; z < _height; z++)
        {
            for (int x = 0; x < _width; x++)
            {
                float nx = 2f * (x / (float)_width) - 1;
                float ny = 2f * (z / (float)_height) - 1;
                float e = HeightMap[x, z];
                //e = (float)Math.Pow(e, 3);
                float d = Distance(nx, ny, DISTANCE_FUNCTIONS.Diagonal);

                if (d < 0) d = 0;
                if (d > 1) d = 1;                

                e = Reshape(e, d);
                e = MathUtils.Clamp(e, 0.0f, 1.0f);          

                FallOffMap[x, z] = e;
            }
        }
        
    }
    
    public float ridgenoise(float value)
    {
        var test = 0.5f - value;
        return 2 * (0.5f - Math.Abs(test));
    }
    
    public float NewNoiseRange(float value)
    {
        float oldMax = maxNoiseHeight;
        float oldMin = minNoiseHeight;
        float newMax = 1.0f;
        float newMin = 0.0f;
        float oldRange = oldMax - oldMin;
        float newRange = newMax - newMin;

        //float value = elevation.GetData(x, z);
        float newValue = (((value - oldMin) * newRange) / oldRange) + newMin;

        return newValue;
    }


    public float Distance(float nx, float ny, DISTANCE_FUNCTIONS funcType)
    {
        switch (funcType)
        {
            case DISTANCE_FUNCTIONS.SquareBump:
                return 1 - (1 - nx * nx) * (1 - ny * ny);
            case DISTANCE_FUNCTIONS.EuclideanSquared:
                return MathUtils.Min(1, (nx * nx + ny * ny) / MathUtils.Sqrt(2));
            case DISTANCE_FUNCTIONS.Diagonal:
                return MathUtils.Max(MathUtils.Abs(nx), MathUtils.Abs(ny));
            case DISTANCE_FUNCTIONS.Manhattan:
                return (MathUtils.Abs(nx) + MathUtils.Abs(ny)) / 2;
            case DISTANCE_FUNCTIONS.Euclidean:
                return MathUtils.Sqrt(nx * nx + ny * ny) / MathUtils.Sqrt(2);
            case DISTANCE_FUNCTIONS.Hyperboloid:
                return (MathUtils.Sqrt(nx * nx + ny * ny + 0.2f * 0.2f) - 0.2f) /
                       (MathUtils.Sqrt(1 * 1 + 1 * 1 + 0.2f * 0.2f) - 0.2f);
            case DISTANCE_FUNCTIONS.Blob:
                return MathUtils.Sqrt(nx * nx + (ny - 0.05f) * (ny - 0.05f)) * MathUtils.Sqrt(2) * 2.7f /
                       (3 - MathUtils.Sin(5 * MathUtils.Atan2(ny - 0.05f, nx)));
            default:
                return 1 - (1 - nx * nx) * (1 - ny * ny);
        }
    }

    public float Reshape(float e, float d)
    {
        return MathUtils.Lerp(e, 1 - d, 0.65f);
    }
}