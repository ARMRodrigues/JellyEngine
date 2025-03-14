namespace JellyEngine;

public class PhysicsBody : GameComponent
{
    public BepuPhysics.Collidables.Collidable Collidable { get; set; }
    public BepuPhysics.RigidPose Pose { get; set; }
    public BepuPhysics.BodyVelocity Velocity { get; set; }
    public Collider CollisionShape { get; set; }
}