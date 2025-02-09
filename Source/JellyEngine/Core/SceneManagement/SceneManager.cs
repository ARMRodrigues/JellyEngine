using System;

namespace JellyEngine.Core.SceneManagement;

public class SceneManager
{
    private readonly Dictionary<string, Scene> _scenes = new();
    private Scene? _currentScene;

    public void AddScene(Scene scene)
    {
        if (!_scenes.TryAdd(scene.Name, scene))
        {
            throw new InvalidOperationException("Scene already exists");
        }
    }

    public void SetActiveScene(Scene scene)
    {
        if (_scenes.ContainsKey(scene.Name))
        {
            _currentScene = scene;
            return;
        }
        
        _scenes[scene.Name] = scene;
        _currentScene = scene;
    }
    
    public void SetActiveScene(string sceneName)
    {
        if (_scenes.TryGetValue(sceneName, out _currentScene))
        {
            throw new InvalidOperationException("Scene does not exist");
        }
    }

    public void InitializeActiveScene()
    {
        _currentScene?.Initialize();
    }
    
    public void UpdateActiveScene()
    {
        _currentScene?.Update();
    }
    
    public void FixedUpdate()
    {
        _currentScene?.FixedUpdate();
    }
    
    public void RenderActiveScene()
    {
        _currentScene?.Render();
    }
    
    public void ShutdownActiveScene()
    {
        _currentScene?.Shutdown();
    }
}
