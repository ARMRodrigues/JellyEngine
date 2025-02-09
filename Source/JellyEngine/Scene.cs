namespace JellyEngine;

public class Scene(string name)
{
    private readonly List<GameSystem> _gameSystems = [];
    
    public string Name { get; private set; } = name;
    public EntityManager EntityManager { get; } = new();

    public void AddGameSystem(GameSystem gameSystem)
    {
        if (_gameSystems.Contains(gameSystem)) return;
        
        _gameSystems.Add(gameSystem);
    }

    public void Initialize()
    {
        foreach (var gameSystem in _gameSystems)
        {
            gameSystem.Initialize();
        }
    }
    
    public void Update()
    {
        foreach (var gameSystem in _gameSystems)
        {
            gameSystem.Update();
        }
    }
    
    public void FixedUpdate()
    {
        foreach (var gameSystem in _gameSystems)
        {
            gameSystem.FixedUpdate();
        }
    }
    
    public void Render()
    {
        foreach (var gameSystem in _gameSystems)
        {
            gameSystem.Render();
        }
    }
    
    public void Shutdown()
    {
        foreach (var gameSystem in _gameSystems)
        {
            gameSystem.Shutdown();
        }
    }
}
