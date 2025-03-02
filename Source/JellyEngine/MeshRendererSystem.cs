using System.Numerics;

namespace JellyEngine;

public class MeshRendererSystem(EntityManager entityManager) : GameSystem
{
    private readonly EntityManager _entityManager = entityManager;
    
    public override void Render()
    {
        foreach (var (entity, transform, meshRenderer) in _entityManager.Query<Transform, MeshRenderer>())
        {
            if (!meshRenderer.IsVisible)
                return;
            
            meshRenderer.Material.Use();
            meshRenderer.Material.SetMVP(transform.WorldMatrix, Camera.Main.ViewMatrix, Camera.Main.ProjectionMatrix);
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