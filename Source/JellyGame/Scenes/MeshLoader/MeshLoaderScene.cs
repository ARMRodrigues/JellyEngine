using System.Numerics;
using JellyEngine;
using JellyGame.Scripts;

namespace JellyGame.Scenes.MeshLoader;

public class MeshLoaderScene : Scene
{
    public MeshLoaderScene(string name) : base(name)
    {
        var environment = new SceneEnvironment();

        var cameraEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(cameraEntity, new Camera(CameraType.Perspective, cameraEntity.Id));
        EntityManager.AddComponent(cameraEntity, new Transform
        {
            LocalPosition = new Vector3(0f, 10f, 15),
            LocalEulerAngles = new Vector3(-10f, 0f, 0f)
        });

        var meshAsset = OBJParser.Load("Assets/Models/Hex.obj");
        var hexEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(hexEntity, new Transform());
        EntityManager.AddComponent(hexEntity, new MeshRenderer(meshAsset.Mesh, meshAsset.Materials));

        var playerEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(playerEntity, new Transform());

        AddGameSystem(new CameraSystem(EntityManager));
        AddGameSystem(new SceneEnvironmentRendererSystem());
        AddGameSystem(new TransformSystem(EntityManager));
        AddGameSystem(new MeshRendererSystem(EntityManager));
        //AddGameSystem(new FreeCameraControllerSystem(EntityManager));
        //AddGameSystem(new ChunkSystem(EntityManager));
    }
}
