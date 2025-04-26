using System.Numerics;
using JellyGame.Scenes.Map2D;

namespace JellyEngine;

public class ChunkSystem : GameSystem
{
    private readonly EntityManager _entityManager;
    private Material _chunkMaterial;
    private Dictionary<(int, int), Entity> _chunks = new();
    private int _lastPlayerChunkX;
    private int _lastPlayerChunkZ;

    public ChunkSystem(EntityManager entityManager)
    {
        _entityManager = entityManager;
        _chunkMaterial = new Material(new Texture("Assets/Textures/Blocks.png")
        {
            FilterMode = FilterMode.Point
        });
    }

    public override void Initialize()
    {
        var chunkComponent = _entityManager.GetComponents<ChunkComponent>().First();
        var playerTransform = _entityManager.GetComponent<Transform>(new Entity(Camera.Main.CameraEntityId));
        
        _lastPlayerChunkX = (int)(playerTransform.LocalPosition.X / chunkComponent.ChunkSize);
        _lastPlayerChunkZ = (int)(playerTransform.LocalPosition.Z / chunkComponent.ChunkSize);
        
        GenerateChunks(_lastPlayerChunkX, _lastPlayerChunkZ, chunkComponent);
    }

    public override void Update()
    {
        var chunkComponent = _entityManager.GetComponents<ChunkComponent>().First();
        var playerTransform = _entityManager.GetComponent<Transform>(new Entity(Camera.Main.CameraEntityId));

        int playerChunkX = (int)(playerTransform.LocalPosition.X / chunkComponent.ChunkSize);
        int playerChunkZ = (int)(playerTransform.LocalPosition.Z / chunkComponent.ChunkSize);

        if (playerChunkX != _lastPlayerChunkX || playerChunkZ != _lastPlayerChunkZ)
        {
            _lastPlayerChunkX = playerChunkX;
            _lastPlayerChunkZ = playerChunkZ;
            GenerateChunks(playerChunkX, playerChunkZ, chunkComponent);
        }
    }

    private void GenerateChunks(int centerX, int centerZ, ChunkComponent chunkComponent)
    {
        var newChunks = new Dictionary<(int, int), Entity>();

        for (int chunkX = -chunkComponent.WorldSizeInChunks; chunkX < chunkComponent.WorldSizeInChunks; chunkX++)
        {
            for (int chunkZ = -chunkComponent.WorldSizeInChunks; chunkZ < chunkComponent.WorldSizeInChunks; chunkZ++)
            {
                int worldX = centerX + chunkX;
                int worldZ = centerZ + chunkZ;
                var chunkKey = (worldX, worldZ);

                if (!_chunks.ContainsKey(chunkKey))
                {
                    var chunkEntity = _entityManager.CreateEntity();
                    _entityManager.AddComponent(chunkEntity, new Transform(new Vector3(worldX * chunkComponent.ChunkSize, 0, worldZ * chunkComponent.ChunkSize)));
                    var chunckMeshRenderer =
                        new MeshRenderer(new ChunkGenV2(chunkComponent.StartChunkPosition).Mesh, _chunkMaterial);
                    _entityManager.AddComponent(chunkEntity, chunckMeshRenderer);
                    newChunks[chunkKey] = chunkEntity;
                }
                else
                {
                    newChunks[chunkKey] = _chunks[chunkKey];
                }
            }
        }

        // Remove chunks that are out of range
        foreach (var chunk in _chunks)
        {
            if (!newChunks.ContainsKey(chunk.Key))
            {
                _entityManager.RemoveEntity(chunk.Value);
            }
        }

        _chunks = newChunks;
    }
}
