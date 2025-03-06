using System.Numerics;
using JellyEngine;
using JellyGame.Scripts;

namespace JellyGame.Scenes.Map2D;

public class Scene2D : Scene
{
    public Scene2D(string name) : base(name)
    {
        var environment = new SceneEnvironment();
            
        var cameraEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(cameraEntity, new Camera(CameraType.Orthographic, cameraEntity.Id)
        {
            OrthographicSize = 3
        });
        EntityManager.AddComponent(cameraEntity, new Transform
        {
            LocalPosition = new Vector3(0f, 0, 10)
        });
        
        var cubeReferenceEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(cubeReferenceEntity, new MeshRenderer(MeshType.Cube, new Material()));
            
        var spriteEntity = EntityManager.CreateEntity();
        var spriteTexture = new Texture("Rat.png")
        {
            FilterMode = FilterMode.Point
        };
        EntityManager.AddComponent(spriteEntity, new Transform()
        {
            LocalScale = Vector3.One * 3f
        });
        EntityManager.AddComponent(spriteEntity, new SpriteRenderer(new Sprite(spriteTexture)));
            
        AddGameSystem(new CameraSystem(EntityManager));
        //AddGameSystem(new SceneEnvironmentRendererSystem());
        AddGameSystem(new TransformSystem(EntityManager));
        AddGameSystem(new SpriteRendererSystem(EntityManager));
        AddGameSystem(new MeshRendererSystem(EntityManager));
    }
}