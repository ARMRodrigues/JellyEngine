using System.Numerics;
using BepuPhysics;

namespace JellyEngine;

public class CharacterController : PhysicsBody
{
    public BodyHandle BodyHandle { get; set; }
    public float Height { get; set; } = 2f;
    public float Radius { get; set; } = 0.5f;
    public float Mass { get; set; } = 1f;
    public Vector3 Suru;
}