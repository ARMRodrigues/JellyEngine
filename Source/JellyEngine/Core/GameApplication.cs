using JellyEngine.Core;
using JellyEngine.Core.Rendering;
using JellyEngine.Core.Rendering.DirectX11;
using JellyEngine.Core.Rendering.OpenGL;

namespace JellyEngine;

public class GameApplication
{
    private readonly RenderContext _rendererContext;

    public GameApplication(NativeWindowSettings nativeWindowSettings)
    {
        var window = new Window(nativeWindowSettings);

        if (nativeWindowSettings.GraphicsAPI == GraphicsAPI.OpenGL)
        {
            _rendererContext = new RenderContext(new OpenGLRenderer(window));
        }
        else
        {
            _rendererContext = new RenderContext(new DirectX11Renderer(window));
        }
    }

    public void Play()
    {
        LifeCycle();
    }

    private void LifeCycle()
    {
        while (_rendererContext.IsWindowOpen())
        {
            _rendererContext.BeginRender();
            _rendererContext.EndRender();
        }
    }
}
