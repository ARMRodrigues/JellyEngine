using System.Numerics;

namespace JellyEngine;

public enum CameraType
{
    Perspective,
    Orthographic
}

public class Camera : GameComponent
{
    public static Camera Main { get; set; } = null!;
    public readonly int CameraEntityId;
    public Matrix4x4 ProjectionMatrix { get; set; }
    public Matrix4x4 ViewMatrix { get; set; }
    public float FieldOfView { get; set; }
    public float NearPlane { get; set; }
    public float FarPlane { get; set; }
    public int OrthographicSize { get; set; }
    public CameraType Type { get; set; }
    
    public Camera(CameraType type, int cameraEntityId)
    {
        Main = this;
        CameraEntityId = cameraEntityId;
        FieldOfView = 60f;
        NearPlane = 0.03f;
        FarPlane = 1000f;
        OrthographicSize = 5;
        Type = type;
    }
    
    public Ray ScreenPointToRay(Vector2 mousePos)
    {
        var x = (2.0f * mousePos.X) / Display.WindowSize.X - 1.0f;
        var y = 1.0f - (2.0f * mousePos.Y) / Display.WindowSize.Y;
        var ndcNear = new Vector4(x, y, 0.0f, 1.0f);
        var ndcFar = new Vector4(x, y, 1.0f, 1.0f);
        
        var viewProj = ViewMatrix * ProjectionMatrix;
        if (!Matrix4x4.Invert(viewProj, out var invViewProj))
        {
            return new Ray(Vector3.Zero, Vector3.UnitZ);
        }

        var worldNear = Vector4.Transform(ndcNear, invViewProj);
        var worldFar = Vector4.Transform(ndcFar, invViewProj);
        
        worldNear /= worldNear.W;
        worldFar /= worldFar.W;
        
        var origin = new Vector3(worldNear.X, worldNear.Y, worldNear.Z);
        var direction = Vector3.Normalize(new Vector3(worldFar.X, worldFar.Y, worldFar.Z) - origin);

        return new Ray(origin, direction);
    }
}