using System.Numerics;

namespace JellyEngine;

public class DirectionalLight
{
    private Vector3 _direction;

    public Vector3 Direction
    {
        get => _direction;
        set
        {
            var pitch = MathUtils.ToDegrees(_direction.X);
            var yaw = MathUtils.ToDegrees(_direction.Y);

            var x = MathF.Cos(pitch) * MathF.Cos(yaw);
            var y = MathF.Sin(pitch);
            var z = MathF.Cos(pitch) * MathF.Sin(yaw);

            _direction =  Vector3.Normalize(new Vector3(x, y, z));
        }
    }
    public Color Color { get; set; }
    public float Intensity { get; set; }

    public DirectionalLight()
    {
        Direction = new Vector3(-60f, 150f, 0);
        Color = Color.White;
        Intensity = 1f;
    }
}