using System.Numerics;

namespace JellyEngine;

public class Transform : GameComponent
{
    private Vector3 _localPosition;
    private Vector3 _localRotation;
    private Vector3 _localScale;
    private Quaternion _rotation;
    private Matrix4x4 _localMatrix;
    private Matrix4x4 _worldMatrix;

    public int ParentId { get; set; }
    public Vector3 LocalPosition
    {
        get => _localPosition;
        set
        {
            _localPosition = value;
            HasTransformValuesChanged = true;
        }
    }
    public Vector3 Position => new Vector3(WorldMatrix.M41, WorldMatrix.M42, WorldMatrix.M43);
    public Vector3 LocalEulerAngles
    {
        get => _localRotation;
        set
        {
            _localRotation = value;
            var radians = Vector3.Multiply(value, (float)(Math.PI / 180.0f));
            _rotation = Quaternion.CreateFromYawPitchRoll(radians.Y, radians.X, radians.Z);
            HasTransformValuesChanged = true;
        }
    }
    public Vector3 LocalScale
    {
        get => _localScale;
        set
        {
            _localScale = value;
            HasTransformValuesChanged = true;
        }
    }
    public Quaternion LocalRotation
    {
        get => _rotation;
        set
        {
            _rotation = value;
            _localRotation = RotationToEulerAngles(_rotation);
            HasTransformValuesChanged = true;
        }
    }
    public Matrix4x4 LocalMatrix
    {
        get
        {
            _localMatrix = Matrix4x4.CreateScale(_localScale) *
                           Matrix4x4.CreateFromQuaternion(_rotation) *
                           Matrix4x4.CreateTranslation(_localPosition);
            return (_localMatrix);
        }
    }
    public Matrix4x4 WorldMatrix
    {
        get => _worldMatrix;
        set
        {
            _worldMatrix = value;
            HasTransformValuesChanged = true;
        }
    }
    public Vector3 Up => Vector3.Normalize(new Vector3(WorldMatrix.M21, WorldMatrix.M22, WorldMatrix.M23));
    public Vector3 Right => Vector3.Normalize(new Vector3(WorldMatrix.M11, WorldMatrix.M12, WorldMatrix.M13));
    public Vector3 Forward => Vector3.Normalize(new Vector3(WorldMatrix.M31, WorldMatrix.M32, WorldMatrix.M33));
    public bool HasTransformValuesChanged { get; private set; }

    public Transform()
    {
        LocalScale = Vector3.One;
        WorldMatrix = LocalMatrix;
    }
    
    // Helper functions
    public static Vector3 RotationToEulerAngles(Quaternion q)
    {
        var pitch = MathF.Asin(2.0f * (q.W * q.X - q.Y * q.Z));
        var yaw = MathF.Atan2(2.0f * (q.W * q.Y + q.Z * q.X), 1 - 2 * (q.X * q.X + q.Y * q.Y));
        var roll = MathF.Atan2(2.0f * (q.W * q.Z + q.X * q.Y), 1 - 2 * (q.Y * q.Y + q.Z * q.Z));
        return new Vector3(pitch, yaw, roll) * (180.0f / MathF.PI);
    }
}