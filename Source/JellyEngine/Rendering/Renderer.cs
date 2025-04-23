namespace JellyEngine.Rendering;

public abstract class Renderer
{
    public abstract bool IsWindowOpen();
    public abstract void BeginRender();
    public abstract void EndRender();
    public abstract void RenderFramebufferToScreen();
    public abstract void PollEvents();
    public abstract void Shutdown();
}
