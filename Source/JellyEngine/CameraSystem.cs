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

            if (camera.Type == CameraType.Perspective)
            {
                camera.ProjectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(
                    MathUtils.ToRadians(camera.FieldOfView),
                    Display.ViewportSize.X / Display.ViewportSize.Y,
                    camera.NearPlane,
                    camera.FarPlane
                );
            }
            else
            {
                var aspectRation = Display.ViewportSize.X / Display.ViewportSize.Y;
                var left = -camera.OrthographicSize * aspectRation;
                var right = camera.OrthographicSize * aspectRation;
                var bottom = -camera.OrthographicSize;
                var top = camera.OrthographicSize;
                camera.ProjectionMatrix = Matrix4x4.CreateOrthographicOffCenter
                (
                    left, right, bottom, top, 
                    camera.NearPlane,camera.FarPlane
                );
            }

            
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