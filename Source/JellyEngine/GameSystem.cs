namespace JellyEngine;

public abstract class GameSystem
{
    public virtual void Initialize() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Render() { }
    public virtual void Shutdown() { }
}