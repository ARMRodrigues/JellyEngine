using System.Numerics;
using JellyEngine;
using JellyEngine.InputManagement;

namespace JellyGame.Scenes.Map2D;

public class SpawnCubeAtRayPointSystem (EntityManager entityManager) : GameSystem
{
    private readonly EntityManager _entityManager = entityManager;
    
    public override void Initialize()
    {
        Input.RegisterAction(new InputAction("Spawn") { MouseButtons = { MouseButton.Left }});
    }

    public override void Update()
    {
        if (Input.IsActionJustPressed("Spawn"))
        {
            var ray = Camera.Main.ScreenPointToRay(Input.MousePosition);
            
            AABB aabb = new AABB(new Vector3(-2.5f, 0, -2.5f), new Vector3(2.5f, 0.1f, 2.5f)); // Caixa de exemplo

            if (aabb.IntersectsRay(ray, out Vector3 intersectionPoint))
            {
                Console.WriteLine($"Intersection at: {intersectionPoint}");
            
                var cube = _entityManager.CreateEntity();
                _entityManager.AddComponent(cube, new Transform(intersectionPoint));
                _entityManager.AddComponent(cube, new MeshRenderer(MeshType.Cube));
            }
            else
            {
                Console.WriteLine("Raio não intersecta AABB.");
            };
            /*var ray = Camera.Main.GetMouseRay(Input.MousePosition);
            var intersectionPoint = ray.Origin + ray.Direction * -10;
            
            var cube = _entityManager.CreateEntity();
            _entityManager.AddComponent(cube, new Transform(intersectionPoint));
            _entityManager.AddComponent(cube, new MeshRenderer(MeshType.Cube));*/
        }
    }
}