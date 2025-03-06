using System.Numerics;

namespace JellyEngine;

public class ChunkComponent : GameComponent
{
    public int WorldSizeInChunks = 5;
    public int ChunkSize = 16;
    public Vector2 StartChunkPosition = Vector2.Zero;
}


