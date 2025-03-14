using System.Numerics;
using JellyEngine;

namespace JellyGame.Scenes.Cubes;

public class CubeSpawnerSystem : GameSystem
{
    private readonly EntityManager _entityManager;
    private readonly Physics _physics;
    private Material _cubeMaterial;
    private int spawnRate = 5;
    private int count = 0;

    public CubeSpawnerSystem(EntityManager entityManager, Physics physics)
    {
        _entityManager = entityManager;
        _physics = physics;
        
        var cubeTexture = new Texture("Assets/Textures/Box.png");
        _cubeMaterial = new Material(cubeTexture);
    }

    public override void Update()
    {
        if (count >= spawnRate)
        {
            var cubeEntity = _entityManager.CreateEntity();
            var cubeRigidBody = new RigidBody();
            var cubeTransform = new Transform()
            {
                LocalPosition = new Vector3(0f, 50f, 0f),
            };
            _entityManager.AddComponent(cubeEntity, cubeTransform);
            _entityManager.AddComponent(cubeEntity, new MeshRenderer(MeshType.Cube, _cubeMaterial));
            _entityManager.AddComponent(cubeEntity, cubeRigidBody);
            _physics.AddBody(cubeRigidBody, cubeTransform);
            
            count = 0;
        }
        
        count++;
    }
}