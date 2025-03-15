using System.Numerics;
using JellyEngine;
using JellyEngine.InputManagement;
using JellyGame.Scenes.Cubes;
using JellyGame.Scenes.Map2D;
using JellyGame.Scripts;

namespace JellyGame.Scenes.WalkAround;

public class WalkAroundScene : Scene
{
    public WalkAroundScene(string name) : base(name)
    {
        var environment = new SceneEnvironment();
        
        var cameraEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(cameraEntity, new Camera(CameraType.Perspective, cameraEntity.Id));
        EntityManager.AddComponent(cameraEntity, new Transform
        {
            LocalPosition = new Vector3(0f, 13f, 26f),
            LocalEulerAngles = new Vector3(-30f, 0f, 0f)
        });
        
        var planeEntity = EntityManager.CreateEntity();
        var planeStaticBody = new StaticBody();
        var planeEntityTransform = new Transform()
        {
            LocalScale = new Vector3(20, 1, 20)
        };
        var planeMaterial = new Material(new Texture("Assets/Textures/Dirt.png")
        {
            FilterMode = FilterMode.Point
        });
        EntityManager.AddComponent(planeEntity, planeEntityTransform);
        EntityManager.AddComponent(planeEntity, new MeshRenderer(MeshType.Plane, planeMaterial));
        EntityManager.AddComponent(planeEntity, planeStaticBody);
        Physics.AddBody(planeStaticBody, planeEntityTransform);
        
        var texture = new Texture("Assets/Textures/Bricks.png");
        var material = new Material(texture);
        
        var leftWallEntity = EntityManager.CreateEntity();
        var leftWallStaticBody = new StaticBody();
        var leftWallTransform = new Transform()
        {
            LocalPosition = new Vector3(-10.5f, 5.5f, 0f),
            LocalScale = new Vector3(1, 10, 20)
        };
        EntityManager.AddComponent(leftWallEntity, leftWallTransform);
        EntityManager.AddComponent(leftWallEntity, new MeshRenderer(MeshType.Cube, material));
        EntityManager.AddComponent(leftWallEntity, leftWallStaticBody);
        Physics.AddBody(leftWallStaticBody, leftWallTransform);
        
        var backWallEntity = EntityManager.CreateEntity();
        var backWallStaticBody = new StaticBody();
        var backWallTransform = new Transform()
        {
            LocalPosition = new Vector3(0, 5.5f, -10.5f),
            LocalScale = new Vector3(20, 10, 1)
        };
        EntityManager.AddComponent(backWallEntity, backWallTransform);
        EntityManager.AddComponent(backWallEntity, new MeshRenderer(MeshType.Cube, material));
        EntityManager.AddComponent(backWallEntity, backWallStaticBody);
        Physics.AddBody(backWallStaticBody, backWallTransform);
        
        var rightWallEntity = EntityManager.CreateEntity();
        var rightWallStaticBody = new StaticBody();
        var rightWallTransform = new Transform()
        {
            LocalPosition = new Vector3(10.5f, 5.5f, 0f),
            LocalScale = new Vector3(1, 10, 20)
        };
        EntityManager.AddComponent(rightWallEntity, rightWallTransform);
        EntityManager.AddComponent(rightWallEntity, new MeshRenderer(MeshType.Cube, material));
        EntityManager.AddComponent(rightWallEntity, rightWallStaticBody);
        Physics.AddBody(rightWallStaticBody, rightWallTransform);
        
        var characterEntity = EntityManager.CreateEntity();
        var characterController = new CharacterController();
        var characterTransform = new Transform(new Vector3(0f, 1f, 0f));
        var characterMaterial = new Material(new Texture());
        characterMaterial.Color = new Color(0f, 0f, 1f);
        EntityManager.AddComponent(characterEntity, characterTransform);
        EntityManager.AddComponent(characterEntity, new MeshRenderer(MeshType.Capsule, characterMaterial));
        EntityManager.AddComponent(characterEntity, new PlayerMovement());
        EntityManager.AddComponent(characterEntity, characterController);
        Physics.AddBody(characterController, characterTransform);
        
        AddGameSystem(new CameraSystem(EntityManager));
        AddGameSystem(new SceneEnvironmentRendererSystem());
        AddGameSystem(new TransformSystem(EntityManager));
        AddGameSystem(new MeshRendererSystem(EntityManager));
        //AddGameSystem(new PhysicsSystem(EntityManager, Physics));
        AddGameSystem(new FreeCameraControllerSystem(EntityManager));
        AddGameSystem(new PlayerMovementSystem(EntityManager));
        //AddGameSystem(new CubeSpawnerSystem(EntityManager, Physics));
        
        Input.RegisterAction(new InputAction("MoveForward") { Keys = { KeyCode.Up } });
        Input.RegisterAction(new InputAction("MoveRight") { Keys = { KeyCode.Right } });
        Input.RegisterAction(new InputAction("MoveBackward") { Keys = { KeyCode.Down } });
        Input.RegisterAction(new InputAction("MoveLeft") { Keys = { KeyCode.Left } });
    }
}