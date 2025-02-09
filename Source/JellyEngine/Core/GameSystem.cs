namespace JellyEngine.Core;

public abstract class GameSystem
{
    public abstract void Initialize();
    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void Render();
    public abstract void Shutdown();
}