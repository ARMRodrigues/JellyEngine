namespace JellyEngine.Rendering.DirectX11;

public class DirectX11Renderer : Renderer
{
    private Window _window;
    
    public DirectX11Renderer(Window window)
    {
        _window = window;
    }

    public override void BeginRender()
    {
        throw new NotImplementedException();
    }

    public override void EndRender()
    {
        throw new NotImplementedException();
    }

    public override void RenderFramebufferToScreen()
    {
        throw new NotImplementedException();
    }

    public override void PollEvents()
    {
        throw new NotImplementedException();
    }

    public override bool IsWindowOpen()
    {
        throw new NotImplementedException();
    }
    
    public override void Shutdown()
    {
        _window.Dispose();
    }
}
