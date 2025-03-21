using System.Numerics;
using JellyEngine;
using JellyEngine.InputManagement;
using JellyGame.Scripts;

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
        var anotherCubeEntity = EntityManager.CreateEntity();
        var spriteEntity = EntityManager.CreateEntity();
        var anotherSpriteEntity = EntityManager.CreateEntity();
        var justanotherSpriteEntity = EntityManager.CreateEntity();
        
        EntityManager.AddComponent(cameraEntity, new Camera(CameraType.Perspective, cameraEntity.Id)
        {
            OrthographicSize = 3
        });
        EntityManager.AddComponent(cameraEntity, new Transform
        {
            LocalPosition = new Vector3(0f, 0f, 10)
        });
        
        EntityManager.AddComponent(cubeEntity, new Transform()
        {
            LocalPosition = new Vector3(0f, 0f, 0)
        });
        EntityManager.AddComponent(cubeEntity, new MeshRenderer(MeshType.Cube, material));
        
        EntityManager.AddComponent(anotherCubeEntity, new Transform()
        {
            LocalPosition = new Vector3(3f, 3f, 0f)
        });
        EntityManager.AddComponent(anotherCubeEntity, new MeshRenderer(MeshType.Cube, material));
        
        EntityManager.AddComponent(armCubeEntity, new Transform
        {
            LocalPosition = new Vector3(0f, 0f, -1f),
            LocalScale = new Vector3(0.5f, 0.5f, 1.0f)
        });
        EntityManager.AddComponent(armCubeEntity, new MeshRenderer(MeshType.Cube));
        
        // Sprites debug
        /*EntityManager.AddComponent(spriteEntity, new Transform()
        {
            LocalScale = Vector3.One * 1.0f
        });
        EntityManager.AddComponent(spriteEntity, new SpriteRenderer(new Sprite("Icon1.png")));

        EntityManager.AddComponent(anotherSpriteEntity, new Transform()
        {
            LocalPosition = new Vector3(-0.5f, 0f, -1f),
            LocalScale = Vector3.One * 1.0f
        });
        EntityManager.AddComponent(anotherSpriteEntity, new SpriteRenderer(new Sprite("Icon1.png")
        {
            Color = new Color(0.6f, 0, 0)
        }));

        EntityManager.AddComponent(justanotherSpriteEntity, new Transform()
        {
            LocalPosition = new Vector3(0.5f, 0f, 1f),
            LocalScale = Vector3.One * 1.0f
        });
        EntityManager.AddComponent(justanotherSpriteEntity, new SpriteRenderer(new Sprite("Icon1.png")
        {
            Color = new Color(0, 0.3f, 0)
        }));*/
        
        EntityManager.AddChild(cubeEntity, armCubeEntity);
        
        AddGameSystem(new CameraSystem(EntityManager));
        AddGameSystem(new TransformSystem(EntityManager));
        AddGameSystem(new MeshRendererSystem(EntityManager));
        AddGameSystem(new ScriptTest());
        
        AddGameSystem(new SpriteRendererSystem(EntityManager));
        
        AddGameSystem(new FreeCameraControllerSystem(EntityManager));
        
        Input.RegisterAction(new InputAction("Jump") { Keys = { KeyCode.Space } });
        Input.RegisterAction(new InputAction("MoveForward") { Keys = { KeyCode.W } });
        Input.RegisterAction(new InputAction("MoveLeft") { Keys = { KeyCode.D } });
        Input.RegisterAction(new InputAction("MoveBackward") { Keys = { KeyCode.S } });
        Input.RegisterAction(new InputAction("MoveRight") { Keys = { KeyCode.A } });
        Input.RegisterAction(new InputAction("Fire") { MouseButtons = {MouseButton.Right} });
    }
}