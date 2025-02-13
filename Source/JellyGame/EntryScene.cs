using System.Numerics;
using JellyEngine;

namespace JellyGame;

public class EntryScene : Scene
{
    public EntryScene(string name) : base(name)
    {
        var boxTexture = new Texture("container2.png");
        var material = new Material(boxTexture);

        var cameraEntity = EntityManager.CreateEntity();
        var armCubeEntity = EntityManager.CreateEntity();
        var cubeEntity = EntityManager.CreateEntity();
        var spriteEntity = EntityManager.CreateEntity();
        var anotherSpriteEntity = EntityManager.CreateEntity();
        
        EntityManager.AddComponent(cameraEntity, new Camera(CameraType.Perspective));
        EntityManager.AddComponent(cameraEntity, new Transform
        {
            LocalPosition = new Vector3(0f, 0f, 5f)
        });
        
        EntityManager.AddComponent(cubeEntity, new Transform()
        {
            LocalPosition = new Vector3(0f, 0f, 0f)
        });
        EntityManager.AddComponent(cubeEntity, new MeshProcessor(MeshType.Cube, material));
        
        EntityManager.AddComponent(armCubeEntity, new Transform
        {
            LocalPosition = new Vector3(0f, 0f, -1f),
            LocalScale = Vector3.One * 0.25f
        });
        EntityManager.AddComponent(armCubeEntity, new MeshProcessor(MeshType.Cube));
        
        EntityManager.AddComponent(spriteEntity, new Transform()
        {
            LocalPosition = new Vector3(0f, 0f, 0f),
            LocalScale = Vector3.One * 1.5f
        });
        EntityManager.AddComponent(spriteEntity, new SpriteRenderer(new Sprite("Oito.png")));

        EntityManager.AddComponent(anotherSpriteEntity, new Transform()
        {
            LocalPosition = new Vector3(0.5f, 0f, -0.1f),
            LocalScale = Vector3.One * 1.5f
        });
        EntityManager.AddComponent(anotherSpriteEntity, new SpriteRenderer(new Sprite("Oito.png")));
        
        EntityManager.AddChild(cubeEntity, armCubeEntity);
        
        AddGameSystem(new CameraSystem(EntityManager));
        AddGameSystem(new TransformSystem(EntityManager));
        AddGameSystem(new MeshRendererSystem(EntityManager));
        
        AddGameSystem(new SpriteRendererSystem(EntityManager));
        
        Console.WriteLine(cubeEntity.Id.ToString());
    }
}