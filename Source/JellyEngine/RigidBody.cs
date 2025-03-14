using BepuPhysics;

namespace JellyEngine;

public class RigidBody : PhysicsBody
{
    public BodyHandle BodyHandle { get; set; }

    private float _mass = 1.0f;
    public float Mass
    {
        get => _mass;
        set => _mass = MathF.Max(0.01f, value);
    }
}
