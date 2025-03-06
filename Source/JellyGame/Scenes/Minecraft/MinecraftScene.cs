using System.Numerics;
using JellyGame.Scripts;

namespace JellyEngine;

public class MinecraftScene : Scene
{
    public MinecraftScene(string name) : base(name)
    {
        var environment = new SceneEnvironment();
        
        var cameraEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(cameraEntity, new Camera(CameraType.Perspective, cameraEntity.Id));
        EntityManager.AddComponent(cameraEntity, new Transform
        {
            LocalPosition = new Vector3(0f, 10f, 15),
            LocalEulerAngles = new Vector3(-10f, 0f, 0f)
        });
        
        var cubeReferenceEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(cubeReferenceEntity, new Transform()
        {
            LocalScale = new Vector3(1, 64, 1)
        });
        EntityManager.AddComponent(cubeReferenceEntity, new MeshRenderer(MeshType.Cube, new Material()));
        
        var anotherCubeReferenceEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(anotherCubeReferenceEntity, new Transform()
        {
            LocalScale = new Vector3(3, 1, 3)
        });
        EntityManager.AddComponent(anotherCubeReferenceEntity, new MeshRenderer(MeshType.Cube, new Material()));
        
        var chunkEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(chunkEntity, new ChunkComponent());
        
        var playerEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(playerEntity, new Transform());
        
        AddGameSystem(new CameraSystem(EntityManager));
        AddGameSystem(new SceneEnvironmentRendererSystem());
        AddGameSystem(new TransformSystem(EntityManager));
        AddGameSystem(new MeshRendererSystem(EntityManager));
        AddGameSystem(new FreeCameraControllerSystem(EntityManager));
        AddGameSystem(new ChunkSystem(EntityManager));
    }
}