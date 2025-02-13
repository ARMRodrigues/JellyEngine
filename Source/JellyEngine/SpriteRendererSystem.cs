namespace JellyEngine;

public class SpriteRendererSystem(EntityManager entityManager) : GameSystem
{
    private readonly EntityManager _entityManager = entityManager;
    
    public override void Render()
    {
        foreach (var (transform, spriteRenderer) in _entityManager.Query<Transform, SpriteRenderer>())
        {
            spriteRenderer.BeginRender();
            spriteRenderer.Material.Use();
            spriteRenderer.Material.SetMVP(transform.WorldMatrix, Camera.Main.ViewMatrix, Camera.Main.ProjectionMatrix);
            spriteRenderer.Render();
        }
    }
}