using System.Numerics;

namespace JellyEngine;

public class PhysicsSystem (EntityManager entityManager, Physics physics) : GameSystem
{
    private readonly EntityManager _entityManager = entityManager;
    private readonly Physics _physics = physics;
    
    public override void Initialize()
    {
    }

    public override void Update()
    {
        
    }

    public override void FixedUpdate()
    {
        _physics.FixedUpdate();
        
        foreach (var (transform, rigidBody) in _entityManager.Query<Transform, RigidBody>() )
        {
            var body = _physics.GetBodyReference(rigidBody.BodyHandle);
            transform.LocalPosition = body.Pose.Position;
            transform.LocalRotation = body.Pose.Orientation;
        }
        
        foreach (var (transform, characterController) in _entityManager.Query<Transform, CharacterController>() )
        {
            var body = _physics.GetBodyReference(characterController.BodyHandle);
            transform.LocalPosition = body.Pose.Position;
            transform.LocalRotation = body.Pose.Orientation;
        }
    }

    public override void Shutdown()
    {

    }
    
}