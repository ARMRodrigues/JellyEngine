namespace JellyEngine;

public class SpriteRendererSystem(EntityManager entityManager) : GameSystem
{
    private readonly EntityManager _entityManager = entityManager;
    private List<QueryResult<Transform, SpriteRenderer>>? _sprites;

    public override void Initialize()
    {
        _sprites = [.. _entityManager
            .Query<Transform, SpriteRenderer>()
            .OrderBy(e => e.Component1.LocalPosition.Z)];
    }

    public override void Render()
    {
        if (_sprites == null)
        {
            return;
        }

        foreach (var (transform, spriteRenderer) in _sprites)
        {
            if (!spriteRenderer.IsVisible)
                return;
            
            spriteRenderer.PrepareRender();
            spriteRenderer.Material.Use();
            spriteRenderer.Material.SetColor(spriteRenderer.Color);
            spriteRenderer.Material.SetMVP(transform.WorldMatrix, Camera.Main.ViewMatrix, Camera.Main.ProjectionMatrix);
            spriteRenderer.Render();
        }
    }
}