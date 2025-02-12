using System.Numerics;

namespace JellyEngine;

public enum CameraType
{
    Perspective,
    Orthographic
}

public class Camera : GameComponent
{
    public static Camera Main { get; set; }
    public Matrix4x4 ProjectionMatrix { get; set; }
    public Matrix4x4 ViewMatrix { get; set; }
    public float FieldOfView { get; set; }
    public float NearPlane { get; set; }
    public float FarPlane { get; set; }
    public int OrthographicSize { get; set; }
    public CameraType Type { get; set; }
    
    public Camera(CameraType type)
    {
        FieldOfView = 60f;
        NearPlane = 0.03f;
        FarPlane = 100f;
        OrthographicSize = 5;
        Type = type;
        Main = this;
    }
}