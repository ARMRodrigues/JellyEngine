namespace JellyEngine.Core.Rendering;

public class RenderContext
{
    private Renderer _renderer;

    public RenderContext(Renderer renderer)
    {
        _renderer = renderer;
    }

    public bool IsWindowOpen()
    {
        return _renderer.IsWindowOpen();
    }

    public void BeginRender()
    {
        _renderer.BeginRender();
    }

    public void EndRender()
    {
        _renderer.EndRender();
    }
}
