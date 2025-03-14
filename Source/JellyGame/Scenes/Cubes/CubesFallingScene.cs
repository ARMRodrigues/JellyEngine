using System.Numerics;
using JellyEngine;
using JellyEngine.InputManagement;

namespace JellyGame.Scenes.Cubes;

public class CubesFallingScene : Scene
{
    public CubesFallingScene(string name) : base(name)
    {
        var environment = new SceneEnvironment();
        
        var cameraEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(cameraEntity, new Camera(CameraType.Perspective, cameraEntity.Id));
        EntityManager.AddComponent(cameraEntity, new Transform
        {
            LocalPosition = new Vector3(0f, 10f, 15f),
            LocalEulerAngles = new Vector3(-30f, 0f, 0f)
        });
        
        var planeEntity = EntityManager.CreateEntity();
        var staticBody = new StaticBody();
        var planeEntityTransform = new Transform()
        {
            LocalScale = new Vector3(20, 1, 20)
        };
        EntityManager.AddComponent(planeEntity, planeEntityTransform);
        EntityManager.AddComponent(planeEntity, new MeshRenderer(MeshType.Cube, new Material()));
        EntityManager.AddComponent(planeEntity, staticBody);
        Physics.AddBody(staticBody, planeEntityTransform);
        
        AddGameSystem(new CameraSystem(EntityManager));
        AddGameSystem(new SceneEnvironmentRendererSystem());
        AddGameSystem(new TransformSystem(EntityManager));
        AddGameSystem(new MeshRendererSystem(EntityManager));
        AddGameSystem(new PhysicsSystem(EntityManager, Physics));
        AddGameSystem(new CubeSpawnerSystem(EntityManager, Physics));
    }
}