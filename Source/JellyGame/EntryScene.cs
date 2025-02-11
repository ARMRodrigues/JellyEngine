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
        
        EntityManager.AddComponent(cameraEntity, new Camera());
        EntityManager.AddComponent(cameraEntity, new Transform
        {
            LocalPosition = new Vector3(0f, 0f, 10f)
        });
        
        EntityManager.AddComponent(cubeEntity, new Transform());
        EntityManager.AddComponent(cubeEntity, new MeshProcessor(MeshType.Cube, material));
        
        EntityManager.AddComponent(armCubeEntity, new Transform
        {
            LocalPosition = new Vector3(0f, 0f, -1f),
            LocalScale = Vector3.One * 0.25f
        });
        EntityManager.AddComponent(armCubeEntity, new MeshProcessor(MeshType.Cube));
        
        EntityManager.AddComponent(spriteEntity, new Transform()
        {
            LocalScale = Vector3.One * 5.0f
        });
        EntityManager.AddComponent(spriteEntity, new SpriteRenderer(new Sprite("Oito.png")));
        
        EntityManager.AddChild(cubeEntity, armCubeEntity);
        
        AddGameSystem(new CameraSystem(EntityManager));
        AddGameSystem(new TransformSystem(EntityManager));
        AddGameSystem(new MeshRendererSystem(EntityManager));
        
        AddGameSystem(new SpriteRendererSystem(EntityManager));
        
        Console.WriteLine(cubeEntity.Id.ToString());
    }
}