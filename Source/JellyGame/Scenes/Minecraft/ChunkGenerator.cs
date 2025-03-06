using System.Numerics;

namespace JellyEngine;

public enum BlockType { Air, Solid }

public enum Direction { Front, Back, Left, Right, Top, Bottom }

public class ChunkGenerator
{
    public const int ChunkSize = 16;
    private BlockType[,,] _blocks; // Armazena os blocos do chunk
    public Mesh Mesh { get; private set; }
    public Vector2 ChunkPosition { get; private set; } 

    public ChunkGenerator(Vector2 chunkPosition)
    {
        ChunkPosition = chunkPosition;
        _blocks = new BlockType[ChunkSize, ChunkSize, ChunkSize];
        GenerateTerrain();
        GenerateMesh();
    }

    private void GenerateTerrain()
    {
        // Exemplo: Gera um terreno plano na altura Y = 8
        for (int x = 0; x < ChunkSize; x++)
        {
            for (int z = 0; z < ChunkSize; z++)
            {
                for (int y = 0; y < ChunkSize; y++)
                {
                    _blocks[x, y, z] = BlockType.Solid;
                }
            }
        }
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
                    if (_blocks[x, y, z] == BlockType.Air) continue;

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
        bool isNeighborSolid = IsInsideBounds(neighbor) && _blocks[(int)neighbor.X, (int)neighbor.Y, (int)neighbor.Z] == BlockType.Solid;

        if (!isNeighborSolid) // Só adiciona a face se o bloco vizinho for ar (vazio)
        {
            foreach (var v in faceVertices)
                positions.Add(v);
            
            Vector2[] faceUVs = GetFaceUVs(direction, BlockType.Solid); // Supondo que o bloco seja do tipo Solid (grama)
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
        // Suponha que o texture atlas seja 4x4, e cada textura ocupe 0.25x0.25 no espaço UV
        float tileSize = 0.25f;
        
        Random random = new Random();
        int textureIndex = random.Next(0, 16);

        // Suponha que a textura de grama seja a primeira textura no atlas (índice 0)
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