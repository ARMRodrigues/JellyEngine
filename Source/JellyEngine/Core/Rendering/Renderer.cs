namespace JellyEngine.Core.Rendering;

public abstract class Renderer
{
    public abstract bool IsWindowOpen();
    public abstract void BeginRender();
    public abstract void EndRender();
    public abstract void Shutdown();
}
