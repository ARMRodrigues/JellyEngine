using System.Numerics;
using JellyEngine;
using JellyGame.Scripts;

namespace JellyGame.Scenes.Guild;

public class GuildManager : Scene
{
    public GuildManager(string name) : base(name)
    {
        var mapSize = new Vector2(10, 10);

        var environment = new SceneEnvironment();

        var cameraEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(cameraEntity, new Camera(CameraType.Perspective, cameraEntity.Id));
        EntityManager.AddComponent(cameraEntity, new Transform
        {
            LocalPosition = new Vector3(0f, 10f, 15),
            LocalEulerAngles = new Vector3(-10f, 0f, 0f)
        });

        /*var chunkEntity = EntityManager.CreateEntity();
        var mesh = ChunkMeshBuilder.Generate();
        EntityManager.AddComponent(chunkEntity, new Transform());
        EntityManager.AddComponent(chunkEntity, new MeshRenderer(mesh , new Material(new Texture("Assets/Textures/Blocks.png"))));
            */

        var map = EntityManager.CreateEntity();
        EntityManager.AddComponent(map, new Transform()
        {
            LocalScale = Vector3.One * 2
        });

        var worldWidth = 200;
        var worldLength = 200;
        var chunkSize = 100;

        var heightMapGenerator = new HeightmapGenerator(worldWidth, worldLength);

        var chunkMap = new ChunkMapBuilder(worldWidth, worldLength, chunkSize, heightMapGenerator);
        var maps = chunkMap.GenerateChunks();

        int chunksX = worldWidth / chunkSize;
        int chunksZ = worldLength / chunkSize;

        int halfChunksX = chunksX / 2;
        int halfChunksZ = chunksZ / 2;

        var chunkMaterial = new Material(new Texture("Assets/Textures/BorderGray.png")
        {
            FilterMode = FilterMode.Point
        });

        for (int cz = -halfChunksZ; cz < halfChunksZ; cz++)
        {
            for (int cx = -halfChunksX; cx < halfChunksX; cx++)
            {
                float worldX = cx * chunkSize;
                float worldZ = cz * chunkSize;

                var chunkEntity = EntityManager.CreateEntity();
                EntityManager.AddComponent(chunkEntity, new Transform
                {
                    LocalPosition = new Vector3(worldX, 0, worldZ)
                });

                if (maps.TryGetValue((cx, cz), out var mesh))
                {
                    EntityManager.AddComponent(chunkEntity, new MeshRenderer(mesh, chunkMaterial));
                }
                EntityManager.AddChild(map, chunkEntity);
            }
        }

        var referenceEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(referenceEntity, new Transform()
        {
            LocalPosition = new Vector3(0f, 0f, 0f),
        });
        EntityManager.AddComponent(referenceEntity, new MeshRenderer(MeshType.Cube, new Material()));

        EntityManager.AddChild(map, referenceEntity);

        var anotherEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(anotherEntity, new Transform()
        {
            LocalPosition = new Vector3(0f, -7f, 0f),
        });
        EntityManager.AddComponent(anotherEntity, new MeshRenderer(MeshType.Cube, new Material()));

        AddGameSystem(new CameraSystem(EntityManager));
        AddGameSystem(new SceneEnvironmentRendererSystem());
        AddGameSystem(new TransformSystem(EntityManager));
        AddGameSystem(new MeshRendererSystem(EntityManager));
        AddGameSystem(new FreeCameraControllerSystem(EntityManager));
    }
}
