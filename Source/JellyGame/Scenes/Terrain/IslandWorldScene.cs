using System.Numerics;
using JellyEngine;
using JellyGame.Scripts;

namespace JellyGame.Scenes.Terrain;

public class IslandWorldScene : Scene
{
    public IslandWorldScene(string name) : base(name)
    {
        var cameraEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(cameraEntity, new Camera(CameraType.Perspective));
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
        EntityManager.AddComponent(referenceEntity, new MeshProcessor(MeshType.Cube, new Material()));

        var ZreferenceEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(ZreferenceEntity, new Transform()
        {
            LocalPosition = -Vector3.UnitZ,
            LocalScale = Vector3.One * 0.5f,
            LocalEulerAngles = Vector3.One * 30f
        });
        EntityManager.AddComponent(ZreferenceEntity, new MeshProcessor(MeshType.Cube, new Material()));
        
        EntityManager.AddChild(referenceEntity, ZreferenceEntity);
        
        var islandGenerator = new IslandGenerator();
        var mesh = islandGenerator.GenerateMeshV2();
        var terrainEntity = EntityManager.CreateEntity();
        var uvTexture = new Texture("uv.png");
        EntityManager.AddComponent(terrainEntity, new Transform());
        EntityManager.AddComponent(terrainEntity, new MeshProcessor(mesh, new Material(uvTexture)));
        
        var borderMesh = islandGenerator.GenerateBorderMesh();
        var edgeterrainEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(edgeterrainEntity, new Transform());
        EntityManager.AddComponent(edgeterrainEntity, new MeshProcessor(borderMesh, new Material(uvTexture)));

        var bottomMesh = islandGenerator.GenerateMirrorMesh();
        var bottomMeshEntity = EntityManager.CreateEntity();
        EntityManager.AddComponent(bottomMeshEntity, new Transform());
        EntityManager.AddComponent(bottomMeshEntity, new MeshProcessor(bottomMesh, new Material(uvTexture)));
        
        AddGameSystem(new CameraSystem(EntityManager));
        AddGameSystem(new TransformSystem(EntityManager));
        AddGameSystem(new MeshRendererSystem(EntityManager));
        AddGameSystem(new FreeCameraControllerSystem(EntityManager));
    }
}