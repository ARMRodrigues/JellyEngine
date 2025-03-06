using System.Numerics;
using JellyEngine;
using JellyEngine.InputManagement;
using JellyGame.Scripts;

namespace JellyGame.Scenes.Terrain;

public class IslandWorldScene : Scene
{
    public IslandWorldScene(string name) : base(name)
    {
        var environment = new SceneEnvironment();
        
        var cameraEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(cameraEntity, new Camera(CameraType.Perspective, cameraEntity.Id));
        EntityManager.AddComponent(cameraEntity, new Transform
        {
            LocalPosition = new Vector3(0f, 3f, 10),
            LocalEulerAngles = new Vector3(-10f, 0f, 0f)
        });
        
        var referenceEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(referenceEntity, new Transform()
        {
            LocalPosition = new Vector3(0f, 0f, -3),
        });
        EntityManager.AddComponent(referenceEntity, new MeshRenderer(MeshType.Cube, new Material()));

        var ZreferenceEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(ZreferenceEntity, new Transform()
        {
            LocalPosition = -Vector3.UnitZ,
            LocalScale = Vector3.One * 0.5f,
            LocalEulerAngles = Vector3.One * 30f
        });
        EntityManager.AddComponent(ZreferenceEntity, new MeshRenderer(MeshType.Cube, new Material()));
        
        EntityManager.AddChild(referenceEntity, ZreferenceEntity);
        
        var islandGenerator = new IslandGenerator();
        var mesh = islandGenerator.GenerateTopMesh();
        var terrainEntity = EntityManager.CreateEntity();
        var uvTexture = new Texture("bw.png");
        EntityManager.AddComponent(terrainEntity, new Transform());
        EntityManager.AddComponent(terrainEntity, new MeshRenderer(mesh, new Material(uvTexture)));
        
        var borderMesh = islandGenerator.GenerateBorderMesh();
        var edgeterrainEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(edgeterrainEntity, new Transform());
        EntityManager.AddComponent(edgeterrainEntity, new MeshRenderer(borderMesh, new Material(uvTexture)));

        /*ar bottomMesh = islandGenerator.GenerateBottomMesh();
        var bottomMeshEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(bottomMeshEntity, new Transform());
        EntityManager.AddComponent(bottomMeshEntity, new MeshProcessor(bottomMesh, new Material(uvTexture)));*/
        
        var waterMeshEntity = EntityManager.CreateEntity();
        var waterlevel = 0.02f * 32f;
        var waterMaterial = new Material() { Color = new Color(1, 0, 0, 0.5f)};
        EntityManager.AddComponent(waterMeshEntity, new Transform()
        {
            LocalPosition = new Vector3(0f, waterlevel, 0f),
            LocalScale = new Vector3(600, 1, 600),
        });
        EntityManager.AddComponent(waterMeshEntity, new MeshRenderer(MeshType.Cube, waterMaterial));

        var noiseTexture = islandGenerator.GenerateNoiseTexture();
        var noiseCanvasTexture = EntityManager.CreateEntity();
        EntityManager.AddComponent(noiseCanvasTexture, new Transform()
        {
            LocalScale = Vector3.One * 2
        });
        EntityManager.AddComponent(noiseCanvasTexture, new CanvasRenderer(new Sprite(noiseTexture)));
        
        AddGameSystem(new CameraSystem(EntityManager));
        AddGameSystem(new SceneEnvironmentRendererSystem());
        AddGameSystem(new TransformSystem(EntityManager));
        AddGameSystem(new MeshRendererSystem(EntityManager));
        AddGameSystem(new FreeCameraControllerSystem(EntityManager));
        AddGameSystem(new CanvasRendererSystem(EntityManager));
        AddGameSystem(new CanvasManagerSystem(EntityManager));
        
        Input.RegisterAction(new InputAction("Map") { Keys = { KeyCode.M } });
    }
}