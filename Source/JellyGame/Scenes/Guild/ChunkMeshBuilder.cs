using JellyEngine;
using System.Numerics;

namespace JellyGame.Scenes.Guild;

public class ChunkMeshBuilder
{
    private static readonly Random _random = new Random();

    private const int ChunkWidth = 100;
    private const int ChunkHeight = 10;
    private const int ChunkLength = 100;

    public static readonly FaceDirection[] Directions =
    {
        new(Direction.Right,  (1, 0, 0)),
        new(Direction.Left,   (-1, 0, 0)),
        new(Direction.Top,    (0, 1, 0)),
        new(Direction.Bottom, (0, -1, 0)),
        new(Direction.Front,  (0, 0, 1)),
        new(Direction.Back,   (0, 0, -1)),
    };

    public static Mesh Generate()
    {
        var mesh = new Mesh();
        var indices = new List<uint>();
        var vertexIndex = 0u;

        var blocks = HeightMap(ChunkWidth, ChunkLength);

        var offsetX = ChunkWidth / 2f;
        var offsetZ = ChunkLength / 2f;

        for (var z = 0; z < ChunkLength; z++)
        {
            for (var x = 0; x < ChunkWidth; x++)
            {
                for (var y = 0; y < ChunkHeight; y++)
                {
                    var block = blocks[x, y, z];
                    if (!block.IsSolid) continue;

                    foreach (var dir in Directions)
                    {
                        var nx = x + dir.Offset.X;
                        var ny = y + dir.Offset.Y;
                        var nz = z + dir.Offset.Z;

                        var neighborSolid = IsInsideBounds(nx, ny, nz) && blocks[nx, ny, nz].IsSolid;
                        if (!neighborSolid)
                        {
                            AddFace(mesh, indices, x - offsetX, y, z - offsetZ, dir, ref vertexIndex, block.Type);
                        }
                    }
                }
            }
        }

        mesh.Indices = indices.ToArray();
        return mesh;
    }

    public static void AddFace(Mesh mesh, List<uint> indices, float x, float y, float z, FaceDirection dir, ref uint vertexIndex, BlockType type)
    {
        var verts = GetFaceVertices(x, y, z, dir.Dir);
        foreach (var v in verts)
        {
            mesh.Positions.Add(v);
            mesh.Colors.Add(GetColorForVoxel(type).ToVector4());
        }

        var normal = CalculateFaceNormal(verts[0], verts[1], verts[2]);
    
        // Adiciona o mesmo normal para os 4 vértices da face (flat shading)
        for (int i = 0; i < 4; i++)
        {
            mesh.Normals.Add(normal);
        }

        var faceUVs = GetFaceUVs(type);
        foreach (var uv in faceUVs) mesh.UV0.Add(uv);

        indices.Add(vertexIndex + 0);
        indices.Add(vertexIndex + 1);
        indices.Add(vertexIndex + 2);
        indices.Add(vertexIndex + 2);
        indices.Add(vertexIndex + 3);
        indices.Add(vertexIndex + 0);

        vertexIndex += 4;
    }

    public static Color GetColorForVoxel(BlockType type)
    {
        return type switch
        {
            BlockType.Ocean => new Color(80, 102, 122),
            BlockType.Shallow => new Color(0.4f, 0.6f, 1f),
            BlockType.Shore => new Color(0.9f, 0.8f, 0.5f),
            BlockType.GrassLow => new Color(139, 147, 110),
            BlockType.GrassMid => new Color(0.4f, 0.8f, 0.2f),
            BlockType.GrassHigh => new Color(0.2f, 0.6f, 0.1f),
            BlockType.Rock => new Color(110, 102, 95),
            BlockType.Ice => new Color(224, 223, 218),
            _ => new Color(224, 223, 218) // Para debug
        };
    }

    private static bool IsInsideBounds(int x, int y, int z) =>
        x >= 0 && x < ChunkWidth &&
        y >= 0 && y < ChunkHeight &&
        z >= 0 && z < ChunkLength;

    private static Vector3[] GetFaceVertices(float x, float y, float z, Direction dir)
    {
        return dir switch
        {
            Direction.Front => new[]
            {
                new Vector3(x,     y,     z + 1),
                new Vector3(x + 1, y,     z + 1),
                new Vector3(x + 1, y + 1, z + 1),
                new Vector3(x,     y + 1, z + 1),
            },
            Direction.Back => new[]
            {
                new Vector3(x + 1, y,     z),
                new Vector3(x,     y,     z),
                new Vector3(x,     y + 1, z),
                new Vector3(x + 1, y + 1, z),
            },
            Direction.Left => new[]
            {
                new Vector3(x, y,     z),
                new Vector3(x, y,     z + 1),
                new Vector3(x, y + 1, z + 1),
                new Vector3(x, y + 1, z),
            },
            Direction.Right => new[]
            {
                new Vector3(x + 1, y,     z + 1),
                new Vector3(x + 1, y,     z),
                new Vector3(x + 1, y + 1, z),
                new Vector3(x + 1, y + 1, z + 1),
            },
            Direction.Top => new[]
            {
                new Vector3(x,     y + 1, z + 1),
                new Vector3(x + 1, y + 1, z + 1),
                new Vector3(x + 1, y + 1, z),
                new Vector3(x,     y + 1, z),
            },
            Direction.Bottom => new[]
            {
                new Vector3(x,     y, z),
                new Vector3(x + 1, y, z),
                new Vector3(x + 1, y, z + 1),
                new Vector3(x,     y, z + 1),
            },
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static Vector3 CalculateFaceNormal(Vector3 v0, Vector3 v1, Vector3 v2)
    {
        Vector3 edge1 = v1 - v0;
        Vector3 edge2 = v2 - v0;
        Vector3 normal = Vector3.Cross(edge1, edge2);
        return Vector3.Normalize(normal);
    }

    private static Vector2[] GetFaceUVs(BlockType type) =>
        new[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1),
        };

    public static Block[,,] HeightMap(int width, int length)
    {
        var blocks = new Block[width, ChunkHeight, length];

        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                float heightNormalized = HeightFunc(x, 0, z);
                BlockType type = GetBlockType(heightNormalized);
                int blockHeight = GetBlockHeight(type);

                for (int y = 0; y < ChunkHeight; y++)
                {
                    blocks[x, y, z] = new Block
                    {
                        Type = y < blockHeight ? type : BlockType.Air
                    };
                }
            }
        }

        return blocks;
    }

    public static Block[,,] HeightMapFromWorld(HeightmapGenerator heightmapGenerator, int startX, int startZ, int chunkWidth, int chunkLength)
    {
        var blocks = new Block[chunkWidth, ChunkHeight, chunkLength];

        for (int z = 0; z < chunkLength; z++)
        {
            for (int x = 0; x < chunkWidth; x++)
            {
                int worldX = startX + x;
                int worldZ = startZ + z;

                float heightNormalized = heightmapGenerator.HeightMap[worldX, worldZ];
                BlockType type = GetBlockType(heightNormalized);
                int blockHeight = GetBlockHeight(type);

                for (int y = 0; y < ChunkHeight; y++)
                {
                    blocks[x, y, z] = new Block
                    {
                        Type = y < blockHeight ? type : BlockType.Air
                    };
                }
            }
        }

        return blocks;
    }

    private static float HeightFunc(int x, int y, int z) => 1;

    private static BlockType GetBlockType(float height) => height switch
    {
        < 0.30f => BlockType.Ocean,
        < 0.40f => BlockType.Shallow,
        < 0.45f => BlockType.Shore,
        < 0.55f => BlockType.GrassLow,
        < 0.65f => BlockType.GrassMid,
        < 0.70f => BlockType.GrassHigh,
        < 0.85f => BlockType.Rock,
        _ => BlockType.Ice,
    };

    private static int GetBlockHeight(BlockType type) => type switch
    {
        BlockType.Ocean => 1,
        BlockType.Shallow => 2,
        BlockType.Shore => 3,
        BlockType.GrassLow => 4,
        BlockType.GrassMid => 5,
        BlockType.GrassHigh => 6,
        BlockType.Rock => 7,
        BlockType.Ice => 8,
        _ => 0
    };
}

public struct FaceDirection
{
    public Direction Dir;
    public (int X, int Y, int Z) Offset;

    public FaceDirection(Direction dir, (int, int, int) offset)
    {
        Dir = dir;
        Offset = offset;
    }
}
