namespace JellyEngine;

public abstract class GameSystem
{
    public void Initialize() { }
    public virtual void Update() { }
    public void FixedUpdate() { }
    public void Render() { }
    public void Shutdown() { }
}