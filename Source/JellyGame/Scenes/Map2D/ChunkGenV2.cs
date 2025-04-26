using System;
using System.Numerics;
using JellyEngine;

namespace JellyGame.Scenes.Map2D;

public enum BlockType
{
    Air = 0,
    Ocean = 1,
    Shallow = 2,
    Shore = 3,
    GrassLow = 4,
    GrassMid = 5,
    GrassHigh = 6,
    Rock = 7,
    Ice = 8
}

public struct Block
{
    public BlockType Type;

    public bool IsSolid => Type != BlockType.Air;
}

public enum Direction { Front, Back, Left, Right, Top, Bottom }

public class ChunkGenV2
{
    public const int ChunkSize = 16;
    private Block[,,] _blocks; // Armazena os blocos do chunk
    public Mesh Mesh { get; private set; }
    public Vector2 ChunkPosition { get; private set; }

    public ChunkGenV2(Vector2 chunkPosition)
    {
        ChunkPosition = chunkPosition;
        _blocks = new Block[ChunkSize, ChunkSize, ChunkSize];
        GenerateTerrain();
        GenerateMesh();
    }

    private void GenerateTerrain()
    {
        FastNoiseLite noise = new FastNoiseLite();
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
        noise.SetFrequency(3f);
        noise.SetFractalType(FastNoiseLite.FractalType.None);

        var scale = 250f;
        var octaves = 5;
        var persistance = 0.5f;
        var lacunarity = 0.4f;

        var octavesOffsets = new Vector2[octaves];

        var offset = new Vector2();

        var prng = new Random();

        for (var i = 0; i < octaves; i++)
        {
            var offsetX = prng.Next(-100000, 100000) + offset.X / 2;
            var offsetY = prng.Next(-100000, 100000) + offset.Y / 2;
            octavesOffsets[i] = new Vector2(offsetX, offsetY);
        }

        int worldHeight = ChunkSize;

        for (int x = 0; x < ChunkSize; x++)
        {
            for (int z = 0; z < ChunkSize; z++)
            {
                var amplitude = 1f;
                var frequency = 1f;
                var noiseHeight = 0f;

                for (var i = 0; i < octaves; i++)
                {
                    var sampleX = (ChunkPosition.X * ChunkSize + x) / scale * frequency + octavesOffsets[i].X * frequency;
                    var sampleZ = (ChunkPosition.Y * ChunkSize + z) / scale * frequency + octavesOffsets[i].Y * frequency;

                    var baseHeight = noise.GetNoise(sampleX, sampleZ);
                    noiseHeight += baseHeight * amplitude;

                    amplitude *= persistance;
                    frequency /= lacunarity;
                }

                ///float height = (noise.GetNoise(ChunkPosition.X * ChunkSize + x, ChunkPosition.Y * ChunkSize + z) + 1) * 0.5f; // Normaliza para [0,1]
                int terrainHeight = (int)(noiseHeight * worldHeight * 0.6f) + 4; // Ajusta a altura do terreno

                for (int y = 0; y < ChunkSize; y++)
                {
                    if (y <= terrainHeight)
                    {
                        BlockType type = (y == terrainHeight)
                            ? GetBlockType((float)y / ChunkSize)
                            : BlockType.Rock; // Subsolo

                        _blocks[x, y, z] = new Block { Type = type };
                    }
                    else
                    {
                        _blocks[x, y, z] = new Block { Type = BlockType.Air };
                    }
                }
            }
        }
    }

    private BlockType GetBlockType(float normalizedHeight)
    {
        if (normalizedHeight < 0.1f)
            return BlockType.Ocean;
        if (normalizedHeight < 0.2f)
            return BlockType.Shallow;
        if (normalizedHeight < 0.3f)
            return BlockType.Shore;
        if (normalizedHeight < 0.5f)
            return BlockType.GrassLow;
        if (normalizedHeight < 0.65f)
            return BlockType.GrassMid;
        if (normalizedHeight < 0.8f)
            return BlockType.GrassHigh;
        if (normalizedHeight < 0.95f)
            return BlockType.Rock;

        return BlockType.Ice;
    }

    private float GetTerrainHeight(float x, float z)
    {
        var noise = new FastNoiseLite();
        noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);

        float frequency = 0.01f;
        float amplitude = 8f;

        float height = noise.GetNoise(x * frequency, z * frequency) * amplitude;
        height += noise.GetNoise(x * (frequency * 2), z * (frequency * 2)) * (amplitude / 2);
        height += noise.GetNoise(x * (frequency * 4), z * (frequency * 4)) * (amplitude / 4);

        return height;
    }


    private void GenerateMesh()
    {
        var positions = new List<Vector3>();
        var indices = new List<uint>();
        var uvs = new List<Vector2>();

        uint vertexIndex = 0;

        for (int x = 0; x < ChunkSize; x++)
        {
            for (int y = 0; y < ChunkSize; y++)
            {
                for (int z = 0; z < ChunkSize; z++)
                {
                    var block = _blocks[x, y, z];
                    if (!block.IsSolid) continue;

                    // Only visible faces
                    AddFaceIfVisible(positions, indices, uvs, ref vertexIndex, x, y, z, Direction.Front);
                    AddFaceIfVisible(positions, indices, uvs, ref vertexIndex, x, y, z, Direction.Back);
                    AddFaceIfVisible(positions, indices, uvs, ref vertexIndex, x, y, z, Direction.Left);
                    AddFaceIfVisible(positions, indices, uvs, ref vertexIndex, x, y, z, Direction.Right);
                    AddFaceIfVisible(positions, indices, uvs, ref vertexIndex, x, y, z, Direction.Top);
                    AddFaceIfVisible(positions, indices, uvs, ref vertexIndex, x, y, z, Direction.Bottom);
                }
            }
        }

        Mesh = new Mesh
        {
            Positions = positions,
            UV0 = uvs,
            Indices = indices.ToArray()
        };
    }

    private void AddFaceIfVisible(List<Vector3> positions, List<uint> indices, List<Vector2> uvs, ref uint vertexIndex, int x, int y, int z, Direction direction)
    {
        Vector3[] faceVertices = GetFaceVertices(x, y, z, direction);
        int[] faceIndices = { 0, 1, 2, 2, 3, 0 };

        Vector3 neighbor = GetNeighborPosition(x, y, z, direction);
        bool isNeighborSolid = IsInsideBounds(neighbor) && _blocks[(int)neighbor.X, (int)neighbor.Y, (int)neighbor.Z].IsSolid;

        if (!isNeighborSolid)
        {
            foreach (var v in faceVertices)
                positions.Add(v);

            Block currentBlock = _blocks[x, y, z];
            Vector2[] faceUVs = GetFaceUVs(direction, currentBlock.Type);
            foreach (var uv in faceUVs)
                uvs.Add(uv);

            foreach (var i in faceIndices)
                indices.Add(vertexIndex + (uint)i);

            vertexIndex += 4;
        }
    }

    private bool IsInsideBounds(Vector3 pos) =>
        pos.X >= 0 && pos.X < ChunkSize &&
        pos.Y >= 0 && pos.Y < ChunkSize &&
        pos.Z >= 0 && pos.Z < ChunkSize;

    private Vector3 GetNeighborPosition(int x, int y, int z, Direction dir) => dir switch
    {
        Direction.Front => new Vector3(x, y, z + 1),
        Direction.Back => new Vector3(x, y, z - 1),
        Direction.Left => new Vector3(x - 1, y, z),
        Direction.Right => new Vector3(x + 1, y, z),
        Direction.Top => new Vector3(x, y + 1, z),
        Direction.Bottom => new Vector3(x, y - 1, z),
        _ => throw new ArgumentOutOfRangeException()
    };

    private Vector3[] GetFaceVertices(int x, int y, int z, Direction dir)
    {
        Vector3 basePos = new(x, y, z);

        return dir switch
        {
            Direction.Front => new[] { basePos + new Vector3(0, 0, 1), basePos + new Vector3(1, 0, 1), basePos + new Vector3(1, 1, 1), basePos + new Vector3(0, 1, 1) },
            Direction.Back => new[] { basePos + new Vector3(1, 0, 0), basePos + new Vector3(0, 0, 0), basePos + new Vector3(0, 1, 0), basePos + new Vector3(1, 1, 0) },
            Direction.Left => new[] { basePos + new Vector3(0, 0, 0), basePos + new Vector3(0, 0, 1), basePos + new Vector3(0, 1, 1), basePos + new Vector3(0, 1, 0) },
            Direction.Right => new[] { basePos + new Vector3(1, 0, 1), basePos + new Vector3(1, 0, 0), basePos + new Vector3(1, 1, 0), basePos + new Vector3(1, 1, 1) },
            Direction.Top => new[] { basePos + new Vector3(0, 1, 0), basePos + new Vector3(0, 1, 1), basePos + new Vector3(1, 1, 1), basePos + new Vector3(1, 1, 0) },
            Direction.Bottom => new[] { basePos + new Vector3(0, 0, 1), basePos + new Vector3(0, 0, 0), basePos + new Vector3(1, 0, 0), basePos + new Vector3(1, 0, 1) },
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private Vector2[] GetFaceUVs(Direction direction, BlockType blockType)
    {
        float tileSize = 0.25f;

        int textureIndex = 13;

        //int textureIndex = (blockType == BlockType.Solid) ? 0 : 1; // Aqui você pode mapear outros tipos de blocos

        int row = textureIndex / 4;
        int col = textureIndex % 4;

        float u = col * tileSize;
        float v = row * tileSize;

        // Retorna as coordenadas de textura para os 4 vértices da face
        return new[]
        {
            new Vector2(u, v),
            new Vector2(u + tileSize, v),
            new Vector2(u + tileSize, v + tileSize),
            new Vector2(u, v + tileSize)
        };
    }
}
