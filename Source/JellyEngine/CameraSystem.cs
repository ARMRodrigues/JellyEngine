using System.Numerics;

namespace JellyEngine;

public class CameraSystem (EntityManager entityManager) : GameSystem
{
    private readonly EntityManager _entityManager = entityManager;
    
    public override void Render()
    {
        foreach (var (transform, camera) in _entityManager.Query<Transform, Camera>())
        {
            camera.ViewMatrix = GetViewMatrix(transform);

            camera.ProjectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(
                MathUtils.ToRadians(camera.FieldOfView),
                Display.ViewportSize.X / Display.ViewportSize.Y,
                camera.NearPlane,
                camera.FarPlane
            );
        }
    }
    
    private static Matrix4x4 GetViewMatrix(Transform transform)
    {
        var position = transform.Position;
        var forward = -transform.Forward;
        var up = transform.Up;

        return Matrix4x4.CreateLookAt(position, position + forward, up);
    }
}