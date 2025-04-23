using JellyEngine.InputManagement;
using JellyEngine.Rendering;
using JellyEngine.Rendering.DirectX11;
using JellyEngine.Rendering.OpenGL;

namespace JellyEngine;

public class GameApplication
{
    private readonly RenderContext _rendererContext;
    private readonly SceneManager _sceneManager;
    private readonly InputSystemManager _inputSystemManager;
    private readonly GameTime _gameTime;

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
        _inputSystemManager = InputSystemManager.Instance;
        _gameTime = new GameTime();
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
            _gameTime.Update();
            _inputSystemManager.UpdateInputStates(InputBackend.GetKeyboardState(), InputBackend.GetMouseState());            
            _sceneManager.UpdateActiveScene();
            _sceneManager.FixedUpdate();
            _rendererContext.BeginRender();
            _sceneManager.RenderActiveScene();
            _rendererContext.EndRender();
            _rendererContext.ApplyPostProcessing();
            _inputSystemManager.ResetState();
            _rendererContext.PollEvents();
        }
        
        _sceneManager.ShutdownActiveScene();
    }
}
