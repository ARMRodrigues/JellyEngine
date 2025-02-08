namespace JellyEngine.Core.Rendering;

public abstract class Renderer
{
    public abstract bool IsWindowOpen();
    public abstract void BeginRender();
    public abstract void EndRender();
}
