using System.Numerics;
using JellyEngine;

namespace JellyGame.Scenes.WalkAround;

public class PlayerMovement : GameComponent
{
    public float Speed { get; set; } = 6f;
    public Vector3 Position { get; set; }
}