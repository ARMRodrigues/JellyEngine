using System.Numerics;
using JellyEngine;
using JellyGame.Scripts;

namespace JellyGame.Scenes.Map2D;

public class RaycastScene : Scene
{
    public RaycastScene(string name) : base(name)
    {
        var environment = new SceneEnvironment();
            
        var cameraEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(cameraEntity, new Camera(CameraType.Perspective, cameraEntity.Id));
        EntityManager.AddComponent(cameraEntity, new Transform
        {
            LocalPosition = new Vector3(0f, 5f, 5),
            LocalEulerAngles = new Vector3(-45f, 0f, 0f),
        });

        var texture = new Texture("Check.png");
        var planeEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(planeEntity, new Transform()
        {
            LocalScale = Vector3.One * 5f
        });
        EntityManager.AddComponent(planeEntity, new MeshRenderer(MeshType.Plane, new Material(texture)));
            
        AddGameSystem(new CameraSystem(EntityManager));
        AddGameSystem(new SceneEnvironmentRendererSystem());
        AddGameSystem(new TransformSystem(EntityManager));
        AddGameSystem(new MeshRendererSystem(EntityManager));
        //AddGameSystem(new FreeCameraControllerSystem(EntityManager));
        AddGameSystem(new SpawnCubeAtRayPointSystem(EntityManager));
    }
}