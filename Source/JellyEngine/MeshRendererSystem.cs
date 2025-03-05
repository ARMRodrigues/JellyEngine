using System.Numerics;

namespace JellyEngine;

public class MeshRendererSystem(EntityManager entityManager) : GameSystem
{
    private readonly EntityManager _entityManager = entityManager;
    
    public override void Render()
    {
        foreach (var (transform, meshRenderer) in _entityManager.Query<Transform, MeshRenderer>())
        {
            if (!meshRenderer.IsVisible)
                return;

            var cameraTransform = _entityManager.GetComponent<Transform>(new Entity(Camera.Main.CameraEntityId));
            var environment = SceneEnvironment.Main;
            
            meshRenderer.Material.Use();
            meshRenderer.Material.SetMatrices(transform.WorldMatrix, Camera.Main.ViewMatrix, Camera.Main.ProjectionMatrix);
            meshRenderer.Material.SetLightData(cameraTransform.Position, environment.DirectionalLight);
            meshRenderer.Render();
        }
    }

    public override void Shutdown()
    {
        foreach (var (entity, transform, meshProcessor) in _entityManager.Query<Transform, MeshRenderer>())
        {
            _entityManager.GetComponent<MeshRenderer>(entity).Dispose();;
        }
    }
}