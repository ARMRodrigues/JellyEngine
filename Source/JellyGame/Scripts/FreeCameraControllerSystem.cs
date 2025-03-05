using System.Numerics;
using JellyEngine;
using JellyEngine.InputManagement;

namespace JellyGame.Scripts;

public class FreeCameraControllerSystem(EntityManager entityManager) : GameSystem
{
    private readonly EntityManager _entityManager = entityManager;

    private Transform? _currentCameraTransform;
    private float _moveSpeed = 0.55f;
    private float _lookSensitivity = 0.1f;
    private float _yaw;
    private float _pitch;

    public override void Initialize()
    {
        Input.RegisterAction(new InputAction("CAMERA_MoveForward") { Keys = { KeyCode.W } });
        Input.RegisterAction(new InputAction("CAMERA_MoveLeft") { Keys = { KeyCode.A } });
        Input.RegisterAction(new InputAction("CAMERA_MoveBackward") { Keys = { KeyCode.S } });
        Input.RegisterAction(new InputAction("CAMERA_MoveRight") { Keys = { KeyCode.D } });
        Input.RegisterAction(new InputAction("CAMERA_MoveUp") { Keys = { KeyCode.E } });
        Input.RegisterAction(new InputAction("CAMERA_MoveDown") { Keys = { KeyCode.Q } });
        Input.RegisterAction(new InputAction("CAMERA_Rotate") { MouseButtons = { MouseButton.Right } });

        Input.SetCursorMode = false;
    }

    public override void Update()
    {
        foreach (var (_, transform, camera) in _entityManager.Query<Transform, Camera>())
        {
            if (camera != Camera.Main) continue;
            _currentCameraTransform = transform;
            break;
        }

        if (_currentCameraTransform == null) return;
        
        var movement = Vector3.Zero;

        if (Input.IsActionPressed("CAMERA_MoveForward"))
            movement -= _currentCameraTransform.Forward;

        if (Input.IsActionPressed("CAMERA_MoveBackward"))
            movement += _currentCameraTransform.Forward;

        if (Input.IsActionPressed("CAMERA_MoveRight"))
            movement += _currentCameraTransform.Right;

        if (Input.IsActionPressed("CAMERA_MoveLeft"))
            movement -= _currentCameraTransform.Right;

        if (Input.IsActionPressed("CAMERA_MoveUp"))
            movement += _currentCameraTransform.Up;

        if (Input.IsActionPressed("CAMERA_MoveDown"))
            movement -= _currentCameraTransform.Up;

        if (movement != Vector3.Zero)
        {
            movement = Vector3.Normalize(movement);
            _currentCameraTransform.LocalPosition += movement * _moveSpeed;
        }

        if (Input.IsActionPressed("CAMERA_Rotate"))
        {
            var mouseDelta = Input.DeltaMousePosition * _lookSensitivity;

            _yaw = _currentCameraTransform.LocalEulerAngles.Y;
            _yaw -= mouseDelta.X;
            
            _pitch -= mouseDelta.Y;
            _pitch = Math.Clamp(_pitch, -89.0f, 89.0f);
            
            _currentCameraTransform.LocalEulerAngles = new Vector3(_pitch, _yaw, 0);
        }
    }
}