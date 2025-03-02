using JellyEngine;
using JellyEngine.InputManagement;

namespace JellyGame.Scenes.Terrain;

public class CanvasManagerSystem (EntityManager entityManager): GameSystem
{
    private readonly EntityManager _entityManager = entityManager;
    
    public override void Update()
    {
        foreach (var canvas in _entityManager.GetComponents<CanvasRenderer>())
        {
            if (Input.IsActionJustPressed("Map"))
            {
                canvas.IsVisible = !canvas.IsVisible;
            }
        }
    }
}