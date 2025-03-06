using System.Numerics;

namespace JellyEngine;

public class ChunkSystem : GameSystem
{
    private readonly EntityManager _entityManager;
    private Material _chunkMaterial;

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
        
        for (int chunkX = -chunkComponent.WorldSizeInChunks; chunkX < chunkComponent.WorldSizeInChunks; chunkX++)
        {
            for (int chunkZ = -chunkComponent.WorldSizeInChunks; chunkZ < chunkComponent.WorldSizeInChunks; chunkZ++)
            {
                var chunkEntity = _entityManager.CreateEntity();
                _entityManager.AddComponent(chunkEntity, new Transform()
                {
                    LocalPosition = new Vector3(chunkX * chunkComponent.ChunkSize, 0, chunkZ * chunkComponent.ChunkSize)
                });
                _entityManager.AddComponent(chunkEntity,
                    new MeshRenderer(new ChunkGenerator(chunkComponent.StartChunkPosition).Mesh, _chunkMaterial));
            }
        }
    }

    public override void Update()
    {
       // TODO: Instantiate new Chunk to create a infinite world based on player position
    }
}