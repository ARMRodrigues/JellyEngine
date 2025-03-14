using System.Numerics;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities.Memory;

namespace JellyEngine;

public class Physics
{
    public readonly Simulation _simulation;
    private static BufferPool _bufferPool;

    public Physics()
    {
        var collisionFilters = new CollidableProperty<SubgroupCollisionFilter>();

        _bufferPool = new BufferPool();

        _simulation = Simulation.Create(
            _bufferPool,
            new NarrowPhaseCallbacks(collisionFilters),
            new PoseIntegratorCallbacks(new Vector3(0, -9.8f, 0)),
            new SolveDescription(1, 1)
        );
    }
    
    public void AddBody(PhysicsBody body, Transform transform)
    {
        if (body is StaticBody staticBody)
        {
            var Size = transform.LocalScale;
            var boxShape = new Box(Size.X, Size.Y, Size.Z);
            var shapeHandle = _simulation.Shapes.Add(boxShape); 

            var collidable = new CollidableDescription(shapeHandle, 0.1f);
            
            var staticDescription = new StaticDescription(
                transform.LocalPosition,  // Position and orientation (RigidPose is the new term in BepuPhysics)
                Quaternion.Identity,
                collidable.Shape   // Shape of the static object
            );

            // Add the static body to the simulation
            staticBody.StaticHandle = _simulation.Statics.Add(staticDescription);
        }
        else if (body is RigidBody rigidBody)
        {
            var Size = transform.LocalScale;
            var boxShape = new Box(Size.X, Size.Y, Size.Z);
            var inertia =  boxShape.ComputeInertia(rigidBody.Mass);
            var shapeHandle = _simulation.Shapes.Add(boxShape); 
            var boxPosition = transform.LocalPosition;
            var boxOrientation = Quaternion.Identity;
            var boxPose = new RigidPose(boxPosition, boxOrientation);
            var boxVelocity = new BodyVelocity();

            var collidable = new CollidableDescription(shapeHandle, 0.1f);
            var boxActivityDescription = new BodyActivityDescription(0.01f);
            var boxBodyDescription = BodyDescription.CreateDynamic(boxPose, boxVelocity, inertia, collidable, boxActivityDescription);

            rigidBody.BodyHandle = _simulation.Bodies.Add(boxBodyDescription);
            
            body.Pose = _simulation.Bodies.GetBodyReference(rigidBody.BodyHandle).Pose;
        }
        else if (body is CharacterController characterController)
        {
            // Crie uma forma de cápsula para o personagem
            var capsuleShape = new Capsule(characterController.Radius, characterController.Height);
            var shapeHandle = _simulation.Shapes.Add(capsuleShape);

            // Calcule a inércia com base na forma e massa
            var inertia = capsuleShape.ComputeInertia(characterController.Mass);

            // Configure a descrição do corpo físico
            var pose = new RigidPose(transform.LocalPosition, Quaternion.Identity);
            var velocity = new BodyVelocity();
            var collidable = new CollidableDescription(shapeHandle, 0.1f);
            var activityDescription = new BodyActivityDescription(0.01f);

            var bodyDescription = BodyDescription.CreateDynamic(pose, velocity, inertia, collidable, activityDescription);

            // Adicione o corpo à simulação
            characterController.BodyHandle = _simulation.Bodies.Add(bodyDescription);

            // Ative o corpo (opcional, dependendo do comportamento desejado)
            _simulation.Awakener.AwakenBody(characterController.BodyHandle);
        }
    }

    public void Awake(BodyHandle bodyHandle)
    {
        _simulation.Awakener.AwakenBody(bodyHandle);
    }

    public BodyReference GetBodyReference(BodyHandle bodyHandle)
    {
        return _simulation.Bodies.GetBodyReference(bodyHandle);
    }

    public void FixedUpdate()
    {
        const float timeStep = 1f / 60f;
        _simulation.Timestep(timeStep);
    }

    void CleanUp()
    {
        _simulation.Dispose();
        _bufferPool.Clear();
    }
}