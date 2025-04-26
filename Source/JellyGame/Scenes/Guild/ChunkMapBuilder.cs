using System;
using JellyEngine;

namespace JellyGame.Scenes.Guild;

public class ChunkMapBuilder
{
    private readonly int worldWidth;
    private readonly int worldLength;
    private readonly int worldHeight;
    private readonly int chunkSize;
    private readonly HeightmapGenerator heightmapGenerator;

    private Block[,,] _blocks;

    public ChunkMapBuilder(int worldWidth, int worldLength, int chunkSize, HeightmapGenerator heightmapGenerator)
    {
        this.worldWidth = worldWidth;
        this.worldLength = worldLength;
        this.worldHeight = Enum.GetValues(typeof(BlockType)).Length;
        this.chunkSize = chunkSize;
        this.heightmapGenerator = heightmapGenerator;
    }

    public Dictionary<(int, int), Mesh> GenerateChunks()
    {
        _blocks = ChunkMeshBuilder.HeightMap(worldWidth, worldLength);
        var chunkMeshes = new Dictionary<(int, int), Mesh>();

        var chunksX = worldWidth / chunkSize;
        var chunksZ = worldLength / chunkSize;

        var halfChunksX = chunksX / 2;
        var halfChunksZ = chunksZ / 2;

        for (var cz = -halfChunksZ; cz < halfChunksZ; cz++)
        {
            for (var cx = -halfChunksX; cx < halfChunksX; cx++)
            {
                var worldStartX = (cx + halfChunksX) * chunkSize;
                var worldStartZ = (cz + halfChunksZ) * chunkSize;

                var mesh = GenerateChunkMesh(worldStartX, worldStartZ);
                chunkMeshes[(cx, cz)] = mesh;
            }
        }

        return chunkMeshes;
    }

    private Mesh GenerateChunkMesh(int startX, int startZ)
    {
        var mesh = new Mesh();
        var indices = new List<uint>();
        var vertexIndex = 0u;

        var blocks = ChunkMeshBuilder.HeightMapFromWorld(heightmapGenerator, startX, startZ, chunkSize, chunkSize);

        for (int z = 0; z < chunkSize; z++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                var worldX = startX + x;
                var worldZ = startZ + z;

                for (int y = 0; y < worldHeight; y++)
                {
                    var block = blocks[x, y, z];
                    if (!block.IsSolid) continue;

                    foreach (var dir in ChunkMeshBuilder.Directions)
                    {
                        var nx = worldX + dir.Offset.X;
                        var ny = y + dir.Offset.Y;
                        var nz = worldZ + dir.Offset.Z;

                        var isNeighborSolid = IsInsideLocalBounds(nx, ny, nz, chunkSize) && blocks[nx, ny, nz].IsSolid;
                        if (!isNeighborSolid)
                        {
                            ChunkMeshBuilder.AddFace(mesh, indices, x, y, z, dir, ref vertexIndex, block.Type);
                        }
                    }
                }
            }
        }

        mesh.Indices = indices.ToArray();
        return mesh;
    }

    private bool IsInsideBounds(int x, int y, int z)
    {
        return x >= 0 && x < worldWidth &&
               y >= 0 && y < worldHeight &&
               z >= 0 && z < worldLength;
    }

    private bool IsInsideLocalBounds(int x, int y, int z, int size)
    {
        return x >= 0 && x < size &&
            y >= 0 && y < worldHeight &&
            z >= 0 && z < size;
    }
}
