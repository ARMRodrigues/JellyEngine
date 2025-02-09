using JellyEngine.Rendering;
using JellyEngine.Rendering.DirectX11;
using JellyEngine.Rendering.OpenGL;

namespace JellyEngine;

public class GameApplication
{
    private readonly RenderContext _rendererContext;
    private readonly SceneManager _sceneManager;

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
        
        _sceneManager = new SceneManager();
    }

    public void Play(Scene scene)
    {
        _sceneManager.SetActiveScene(scene);
        
        LifeCycle();
    }

    private void LifeCycle()
    {
        _sceneManager.InitializeActiveScene();
        
        while (_rendererContext.IsWindowOpen())
        {
            _rendererContext.BeginRender();
            _sceneManager.UpdateActiveScene();
            _sceneManager.FixedUpdate();
            _sceneManager.RenderActiveScene();
            _rendererContext.EndRender();
        }
        
        _sceneManager.ShutdownActiveScene();
    }
}
