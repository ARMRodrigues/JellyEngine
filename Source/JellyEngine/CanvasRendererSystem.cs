using System.Numerics;

namespace JellyEngine;

public class CanvasRendererSystem (EntityManager entityManager) : GameSystem
{
    private readonly EntityManager _entityManager = entityManager;
    private List<QueryResult<Transform, CanvasRenderer>>? _sprites;
    
    private Matrix4x4 OrtoProjectionMatrix;

    public override void Initialize()
    {
        _sprites = [.. _entityManager
            .Query<Transform, CanvasRenderer>()
            .OrderBy(e => e.Component1.LocalPosition.Z)];
        
        var aspectRation = Display.ViewportSize.X / Display.ViewportSize.Y;
        var left = -Camera.Main.OrthographicSize * aspectRation;
        var right = Camera.Main.OrthographicSize * aspectRation;
        var bottom = -Camera.Main.OrthographicSize;
        var top = Camera.Main.OrthographicSize;
        OrtoProjectionMatrix = Matrix4x4.CreateOrthographicOffCenter
        (
            left, right, bottom, top, 
            Camera.Main.NearPlane, Camera.Main.FarPlane
        );
    }

    public override void Render()
    {
        if (_sprites == null)
        {
            return;
        }

        foreach (var (transform, canvasRenderer) in _sprites)
        {
            if (!canvasRenderer.IsVisible)
                return;
            canvasRenderer.PrepareRender();
            canvasRenderer.Material.Use();
            canvasRenderer.Material.SetColor(canvasRenderer.Color);
            canvasRenderer.Material.SetMVP(transform.WorldMatrix, Matrix4x4.Identity, OrtoProjectionMatrix);
            canvasRenderer.Render();
        }
    }
}