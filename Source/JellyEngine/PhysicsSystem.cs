using System.Numerics;
using BepuUtilities;

namespace JellyEngine;

public class PhysicsSystem (EntityManager entityManager, Physics physics) : GameSystem
{
    private readonly EntityManager _entityManager = entityManager;
    private readonly Physics _physics = physics;
    
    public override void Initialize()
    {
        foreach (var (transform, characterController) in _entityManager.Query<Transform, CharacterController>() )
        {
            _physics.Awake(characterController.BodyHandle);
        }
    }

    public override void Update()
    {
        foreach (var (transform, rigidBody) in _entityManager.Query<Transform, RigidBody>() )
        {
            var body = _physics.GetBodyReference(rigidBody.BodyHandle);
            transform.LocalPosition = body.Pose.Position;
            transform.LocalRotation = body.Pose.Orientation;
        }
        
        foreach (var (transform, characterController) in _entityManager.Query<Transform, CharacterController>() )
        {
            var body = _physics.GetBodyReference(characterController.BodyHandle);
            var y = body.Pose.Position.Y;
            transform.LocalPosition = body.Pose.Position;
            //transform.LocalRotation = body.Pose.Orientation;

        }
    }

    public override void FixedUpdate()
    {
        _physics.FixedUpdate();
        
        foreach (var (transform, characterController) in _entityManager.Query<Transform, CharacterController>() )
        {
            _physics.Awake(characterController.BodyHandle);
            
            var body = _physics.GetBodyReference(characterController.BodyHandle);
            var y = body.Velocity.Linear.Y;
            body.Velocity.Linear = new Vector3(characterController.Suru.X, y, characterController.Suru.Z);
            transform.LocalRotation = Quaternion.Identity;
            body.LocalInertia.InverseInertiaTensor = new Symmetric3x3(); // Sem rotação

            
            //transform.LocalPosition = body.Pose.Position;
            //transform.LocalRotation = body.Pose.Orientation;
        }
    }

    public override void Shutdown()
    {

    }
    
}