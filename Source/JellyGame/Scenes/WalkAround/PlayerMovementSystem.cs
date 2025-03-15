using System.Numerics;
using JellyEngine;
using JellyEngine.InputManagement;

namespace JellyGame.Scenes.WalkAround;

public class PlayerMovementSystem (EntityManager entityManager) : GameSystem
{
    private readonly EntityManager _entityManager = entityManager;
    
    private Transform _playerTransform;
    private PlayerMovement _playerMovement;
    private CharacterController _characterController;

    public override void Initialize()
    {
        foreach (var (entity, transform, playerMovement) in _entityManager.Query<Transform, PlayerMovement>())
        {
            _playerTransform = transform;
            _playerMovement = playerMovement;
            break;
        }
        
        foreach (var (entity, transform, character) in _entityManager.Query<Transform, CharacterController>())
        {
            _characterController = character;
            break;
        }

        //_playerMovement.Position = _playerTransform.LocalPosition;
    }

    public override void Update()
    {
        Move();
    }

    private void Move()
    {
            var newPos = Vector3.Zero;

            if (Input.IsActionPressed("MoveForward"))
            {
                newPos -= Vector3.UnitZ * GameTime.DeltaTime * _playerMovement.Speed;
            }

            if (Input.IsActionPressed("MoveRight"))
            {
                newPos -= Vector3.UnitX * GameTime.DeltaTime * _playerMovement.Speed;
            }

            if (Input.IsActionPressed(""))
            {
                newPos += Vector3.UnitZ * GameTime.DeltaTime * _playerMovement.Speed;
            }

            if (Input.IsActionPressed("MoveLeft"))
            {
                newPos += Vector3.UnitX * GameTime.DeltaTime * _playerMovement.Speed;
            }
            
            var moveDirection = Vector3.Zero;
            if (Input.IsActionPressed("MoveForward")) moveDirection.Z -= 1; // Frente
            if (Input.IsActionPressed("MoveBackward")) moveDirection.Z += 1; // Trás
            if (Input.IsActionPressed("MoveLeft")) moveDirection.X -= 1; // Esquerda
            if (Input.IsActionPressed("MoveRight")) moveDirection.X += 1; // Direita

            // Normaliza a direção do movimento
            if (moveDirection != Vector3.Zero)
            {
                moveDirection = Vector3.Normalize(moveDirection);
            }
            
            var targetVelocity = moveDirection * _playerMovement.Speed;

            //var worldDirection = _playerTransform.GetRight() * newPos.X + _playerTransform.GetUp() * newPos.Y + _playerTransform.GetForward() * newPos.Z;

            //Console.WriteLine(worldDirection);

            //if (newPos.LengthSquared() > 0)
             //   _playerMovement.Position += newPos;
             
             
            //_playerTransform.LocalPosition += targetVelocity;
            _characterController.Suru = (targetVelocity);
        
    }
}