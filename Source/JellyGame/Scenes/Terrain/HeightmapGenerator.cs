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

    private readonly int _size;
    private readonly FastNoiseLite _noise;

    public HeightmapGenerator(int newSize, FastNoiseLite noise)
    {
        _size = newSize + 1;
        _noise = noise;
       
        HeightMap = new float[_size, _size];
        FallOffMap = new float[_size, _size];

        GenerateHeightmap();
    }

    public void GenerateHeightmap()
    {
        for (int z = 0; z < _size; z++)
        {
            for (int x = 0; x < _size; x++)
            {
                var y = _noise.GetNoise(x, z);
                HeightMap[x, z] = y;
            }
        }

        for (int z = 0; z < _size; z++)
        {
            for (int x = 0; x < _size; x++)
            {
                float nx = 2f * (x / (float)_size) - 1;
                float ny = 2f * (z / (float)_size) - 1;
                float e = HeightMap[x, z];
                float d = Distance(nx, ny, DISTANCE_FUNCTIONS.SquareBump);

                if (d < 0) d = 0;
                if (d > 1) d = 1;

                e = Reshape(e, d);
                e = MathUtils.Clamp(e, 0.0f, 1.0f);
                FallOffMap[x, z] = e;
            }
        }
        
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